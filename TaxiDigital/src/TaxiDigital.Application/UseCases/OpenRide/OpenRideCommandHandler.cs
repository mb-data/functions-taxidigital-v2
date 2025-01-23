using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaxiDigital.Application.Common.Messaging;
using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Application.Services.DriverLog;
using TaxiDigital.Application.Services.ExpoData;
using TaxiDigital.Application.Services.TaxiDigital;
using TaxiDigital.Domain.Authorize.Requests;
using TaxiDigital.Domain.Authorize.Results;
using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.Domain.Ride.Requests;
using TaxiDigital.Domain.Ride.Results;
using TaxiDigital.Domain.User.Requests;
using TaxiDigital.Domain.User.Results;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.UseCases.OpenRide;

internal sealed class OpenRideCommandHandler(
    ILogger<OpenRideCommandHandler> logger,
    IRideService rideService,
    IProviderService providerService,
    ICompanyService companyService,
    IUserService userService,
    ITaxiDigitalService taxiDigitalService,
    IUserProviderService userProviderService,
    IDriverLogService driverLogService) : ICommandHandler<OpenRideCommand>
{
    private readonly ILogger<OpenRideCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IRideService _rideService = rideService ?? throw new ArgumentNullException(nameof(rideService));
    private readonly IProviderService _providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));
    private readonly ICompanyService _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    private readonly ITaxiDigitalService _taxiDigitalService = taxiDigitalService ?? throw new ArgumentNullException(nameof(taxiDigitalService));
    private readonly IUserProviderService _userProviderService = userProviderService ?? throw new ArgumentNullException(nameof(userProviderService));
    private readonly IDriverLogService _driverLogService = driverLogService ?? throw new ArgumentNullException(nameof(driverLogService));

    public async Task<Result> Handle(OpenRideCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"OpenRideCommandHandler start processing Ride ID: {request.RideId}");

        RideEstimativeResult selectedEstimative = await _rideService.GetBestEstimative(request.RideId);
        int providerId = selectedEstimative.Product.ProviderID;
        RideResult ride = await _rideService.Get(request.RideId);

        try
        {
            await _rideService.PostFunction(request.RideId, new RideProviderFunctionRequest(providerId, 2));
            List<ProviderResult> providers = await _providerService.Get(null, providerId);

            string companyToken = await _companyService.GetTaxiDigitalToken(ride.CompanyID, providers[0].ProviderConfigurations.Find(o => o.ProviderConfigurationTypeID == 1).Value);
            string companyHasRegistration = await _companyService.GetTaxiDigitalToken(ride.CompanyID, "64");

            if (string.IsNullOrEmpty(companyToken))
            {
                await _rideService.UpdateFunctionLog(request.RideId, "The Taxi Digital token is not configured", providerId);
                return Result.Failure(new Error("TaxiDigitalTokenNotFound", "The Taxi Digital token is not configured", ErrorType.Failure));
            }

            RideAddressResult startAddress = ride.RideAdresses.FirstOrDefault(o => o.RideAddressTypeID == 1);
            RideAddressResult endAddress = ride.RideAdresses.FirstOrDefault(o => o.RideAddressTypeID == 2);
            UserResult user = await _userService.Get(ride.UserID);

            if (string.IsNullOrEmpty(companyHasRegistration))
                user.Registration = user.Email;

            AuthorizedRequest authorizedRequest = new AuthorizedRequest
            {
                user_email = user.Email,
                user_name = user.Name,
                user_phone = new string(user.Phone.Where(char.IsDigit).ToArray()),
                classificador1 = user.Registration != null ? user.Registration : user.Email,
                classificador2 = user.Registration != null ? user.Registration : user.Email,
                classificador3 = user.Registration != null ? user.Registration : user.Email,
                classificador4 = user.Registration != null ? user.Registration : user.Email,
                classificador5 = user.Registration != null ? user.Registration : user.Email,
                classificador6 = user.Registration != null ? user.Registration : user.Email,
                classificador7 = user.Registration != null ? user.Registration : user.Email,
                classificador8 = user.Registration != null ? user.Registration : user.Email,
                classificador9 = user.Registration != null ? user.Registration : user.Email,
                classificador10 = user.Registration != null ? user.Registration : user.Email,
                classificador11 = user.Registration != null ? user.Registration : user.Email,
                classificador12 = user.Registration != null ? user.Registration : user.Email,
                classificador13 = user.Registration != null ? user.Registration : user.Email
            };

            CreateAuthorizedResult createAuthorizedResult = await _taxiDigitalService.CreateAuthorized(authorizedRequest, companyToken);

            if (!string.IsNullOrEmpty(createAuthorizedResult?.authorized_id))
            {
                UserProviderConfigurationResult userProviderConfigurationResult = await _userProviderService.Post(user.UserID, new UserProviderConfigurationRequest(providerId, 1, createAuthorizedResult.authorized_id));
                _logger.LogInformation($"UserProviderConfiguration: {JsonSerializer.Serialize(userProviderConfigurationResult)}");
            }
            else
            {
                _logger.LogWarning($"ID not found: {user.UserID}");
            }

            BookRideRequest bookRideRequest = new BookRideRequest
            {
                user_email = user.Email,
                user_phone = user.Phone,
                user_name = user.Name,
                init_address_name = startAddress.Address,
                init_address_lat = double.Parse(startAddress.Latitude, CultureInfo.InvariantCulture),
                init_address_lng = double.Parse(startAddress.Longitude, CultureInfo.InvariantCulture),
                init_address_number = startAddress.Number,
                end_address_name = endAddress.Address,
                end_address_lat = double.Parse(endAddress.Latitude, CultureInfo.InvariantCulture),
                end_address_lng = double.Parse(endAddress.Longitude, CultureInfo.InvariantCulture),
                payment_id = 2,
                schedule = 0,
                unique_field = user.Registration != null ? user.Registration : user.Email,
                category_id = int.Parse(selectedEstimative.ProductID.Replace("TAXID ", string.Empty)),
                booking_hash = ride.RideID.ToString(),
                estimate_fare = selectedEstimative.Price.ToString(CultureInfo.InvariantCulture),
                external_authorization_id = ride.RideID.ToString()
            };

            var logRequest = new DriverLogRequest
            {
                Lat = null,
                Lng = null,
                RideID = ride.RideID,
                RideProviderID = ride?.RideProviderID,
                ProductID = null,
                ProviderID = ride.ProviderID,
                Observation = JsonSerializer.Serialize(request).ToString(),
                RideStatusID = ride.RideStatusID,
                Method = "OPEN RIDE"
            };

            await _driverLogService.Post(logRequest);

            _logger.LogInformation($"OpenRideRequest - {ride.RideID} : {JsonSerializer.Serialize(bookRideRequest)}");
            BookRideResult bookRideResult = await _taxiDigitalService.BookRide(bookRideRequest, companyToken);
            _logger.LogInformation($"OpenRide - {ride.RideID} : {JsonSerializer.Serialize(bookRideResult)}");

            RideResult rideResult = await _rideService.Put(request.RideId, new RideRequest(5,selectedEstimative.Product.ProviderID, bookRideResult.data.ToString()));
            _logger.LogInformation($"RideResult: {JsonSerializer.Serialize(rideResult)}");
            
            await _rideService.UpdateFunctionLog(request.RideId, null, providerId);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EstimateQueueCommandHandler");
            await _rideService.UpdateFunctionLog(request.RideId, JsonSerializer.Serialize(ex).ToString(), providerId);

            DriverLogRequest driverLogRequest = new DriverLogRequest
            {
                Lat = null,
                Lng = null,
                RideID = request.RideId,
                RideProviderID = ride?.RideProviderID,
                ProductID = null,
                ProviderID = ride.ProviderID,
                Observation = JsonSerializer.Serialize(ex).ToString(),
                RideStatusID = ride.RideStatusID,
                Method = "OPEN RIDE"
            };

            await _driverLogService.Post(driverLogRequest);

            return Result.Failure(new Error("EstimateQueueCommandHandler", ex.Message, ErrorType.Failure));
        }
    }
}
