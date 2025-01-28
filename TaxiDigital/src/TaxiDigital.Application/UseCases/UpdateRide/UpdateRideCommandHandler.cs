using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using TaxiDigital.Application.Common.Messaging;
using TaxiDigital.Application.Common.Storage;
using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Application.Services.ExpoData;
using TaxiDigital.Application.Services.TaxiDigital;
using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.Domain.Notification.Requests;
using TaxiDigital.Domain.Ride.Requests;
using TaxiDigital.Domain.Ride.Results;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.UseCases.UpdateRide;

internal sealed class UpdateRideCommandHandler(
    ILogger<UpdateRideCommandHandler> logger,
    IRideService rideService,
    IProviderService providerService,
    ICompanyService companyService,
    IStorageService storageService,
    ITaxiDigitalService taxiDigitalService,
    IDriverLogService driverLogService) : ICommandHandler<UpdateRideCommand>
{
    private const int ProviderConfigurationTypeID = 1;
    private const int RideStatusCanceled = 3;
    private const int RideStatusOpen = 5;
    private const int RideStatusDriverInTransit = 6;
    private const int RideStatusAtLocation = 7;
    private const int RideStatusStarted = 8;
    private const int RideStatusFinished = 9;

    private readonly ILogger<UpdateRideCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IRideService _rideService = rideService ?? throw new ArgumentNullException(nameof(rideService));
    private readonly IProviderService _providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));
    private readonly ICompanyService _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
    private readonly IStorageService _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
    private readonly ITaxiDigitalService _taxiDigitalService = taxiDigitalService ?? throw new ArgumentNullException(nameof(taxiDigitalService));
    private readonly IDriverLogService _driverLogService = driverLogService ?? throw new ArgumentNullException(nameof(driverLogService));

    public async Task<Result> Handle(UpdateRideCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Getting ride with external authorization id {request.ExternalAuthorizationId}");
        var ride = await _rideService.Get(int.Parse(request.ExternalAuthorizationId));

        _logger.LogInformation($"Getting provider with id {ride.ProviderID}");
        var providers = await _providerService.Get(null, ride.ProviderID);

        if (!providers.Any())
        {
            _logger.LogError($"Providers with id {ride.ProviderID} not found");
            return Result.Failure(new Error("ProviderNotFound", "Provider not found", ErrorType.NotFound));
        }

        var providerConfig = providers[0].ProviderConfigurations
            .FirstOrDefault(x => x.ProviderConfigurationTypeID == ProviderConfigurationTypeID);

        if (providerConfig == null || string.IsNullOrEmpty(providerConfig.Value))
        {
            _logger.LogError($"Provider not found for ProviderConfigurationTypeID == {ProviderConfigurationTypeID}");
            return Result.Failure(new Error("ProviderNotFound", "Provider not found", ErrorType.NotFound));
        }

        string provider = providerConfig.Value;
        string companyToken = await _companyService.GetTaxiDigitalToken(ride.CompanyID, provider);

        if (string.IsNullOrEmpty(companyToken))
        {
            _logger.LogError($"Company token not found for company id {ride.CompanyID}");
            return Result.Failure(new Error("CompanyTokenNotFound", "Company token not found", ErrorType.NotFound));
        }

        if (ride.OpenRideDate != null)
        {
            if ((DateTime.Now.AddMinutes(-5) >= ride.OpenRideDate && ride.RideStatusID == RideStatusOpen) || ride.RideStatusID == RideStatusCanceled)
            {
                await _storageService.AddMessage("cancel-taxidigital", ride.RideID.ToString());
                return Result.Success();
            }
        }

        if (ride.RideStatusID == RideStatusCanceled && !string.IsNullOrEmpty(ride.RideProviderID))
        {
            await _storageService.AddMessage("cancel-taxidigital", ride.RideID.ToString());
        }

        RideInfoResult rideInfoResult;
        try
        {
            rideInfoResult = await _taxiDigitalService.RideInfo(new RideInfoRequest(request.BookingId), companyToken);
            _logger.LogInformation($"UpdateRide - RideID: {ride.RideID}, BookingID: {request.BookingId}, RideInfo: {JsonSerializer.Serialize(rideInfoResult)}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to fetch ride info for BookingID: {request.BookingId}");
            return Result.Failure(new Error("RideInfoFetchFailed", "Failed to fetch ride info", ErrorType.OperationFailed));
        }

        var rideEstimativeResult = await _rideService.GetBestEstimative(ride.RideID);
        int providerId = rideEstimativeResult.Product.ProviderID;

        var rideRequest = new RideRequest();

        if (request.Type.Equals("status", StringComparison.OrdinalIgnoreCase))
        {
            var rideStatusResult = new RideStatusResult
            {
                booking_id = request.BookingId,
                type = request.Type,
                external_authorization_id = request.ExternalAuthorizationId
            };

            _logger.LogInformation($"Status => {JsonSerializer.Serialize(rideStatusResult)}");

            var sendNotificationRequest = new SendNotificationRequest { RideStatusID = 0 };

            rideRequest.RideStatusID = rideStatusResult.status switch
            {
                0 => RideStatusCanceled,
                1 => RideStatusOpen,
                7 => RideStatusDriverInTransit,
                3 => RideStatusAtLocation,
                4 => RideStatusStarted,
                5 => RideStatusFinished,
                _ => 0 // Default case
            };

            sendNotificationRequest.RideStatusID = rideRequest.RideStatusID;

            var driverLogRequest = CreateDriverLogRequest(ride, "UPDATE RIDE STATUS", rideRequest.RideStatusID, JsonSerializer.Serialize(rideInfoResult));
            await _driverLogService.Post(driverLogRequest);

            if (rideRequest.RideStatusID >= RideStatusDriverInTransit)
            {
                rideRequest.Car = rideInfoResult.data.car_brand + " " + rideInfoResult.data.car_model;
                rideRequest.Plate = rideInfoResult.data.license_plate;
                rideRequest.Driver = rideInfoResult.data.driver_name;
                rideRequest.DriverPicture = rideInfoResult.data.driver_photo;
                rideRequest.DriverPhone = rideStatusResult.data?.phone_number ?? ride.DriverPhone;
            }

            if (ride.RideStatusID != 0)
            {
                rideRequest.Price = rideEstimativeResult.Price;
                await _rideService.Put(ride.RideID, rideRequest);
            }

            if (rideRequest.RideStatusID == RideStatusFinished)
            {
                await _rideService.PostFunction(ride.RideID, new RideProviderFunctionRequest(DateTime.Now, providerId, 4));
            }

            if (sendNotificationRequest.RideStatusID != 0)
            {
                sendNotificationRequest.Driver = rideInfoResult.data?.driver_name ?? "";
                sendNotificationRequest.Vehicle = rideInfoResult.data?.car_brand + " " + rideInfoResult.data?.car_model ?? "";
                sendNotificationRequest.Plate = rideInfoResult.data?.license_plate ?? "";
                sendNotificationRequest.DriverPhone = rideStatusResult.data?.phone_number ?? "";
                sendNotificationRequest.WaitingTime = rideInfoResult.data?.informed_time ?? "";
                sendNotificationRequest.Price = ride.Price.ToString("N2");
                sendNotificationRequest.Updated = ride.Updated.AddHours(-3).ToString("dd/MM/yyyy HH:mm");
                sendNotificationRequest.Provider = providers[0].Description;
                sendNotificationRequest.ProviderID = ride.ProviderID;
                sendNotificationRequest.Origin = ride.RideAdresses?.Find(x => x.RideAddressTypeID == 1).Address ?? "";
                sendNotificationRequest.Destination = ride.RideAdresses?.Find(x => x.RideAddressTypeID == 2).Address ?? "";
                sendNotificationRequest.UserID = ride.UserID;
                sendNotificationRequest.RideID = ride.RideID;
                sendNotificationRequest.Date = ride.Create.AddHours(-3).ToString("dd/MM/yyyy");

                var plainTextBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));
                var requestBase64 = Convert.ToBase64String(plainTextBytes);

                _logger.LogInformation($"NotificationRequest - {ride.RideID}: {JsonSerializer.Serialize(sendNotificationRequest)}");
                await _storageService.AddMessage("send-driver-notification", requestBase64);
            }
        }
        else if (request.Type.Equals("position", StringComparison.OrdinalIgnoreCase))
        {
            var ridePositionResult = new RidePositionResult
            {
                booking_id = request.BookingId.ToString(),
                type = request.Type,
                external_authorization_id = request.ExternalAuthorizationId
            };

            _logger.LogInformation($"Position => {JsonSerializer.Serialize(ridePositionResult)}");
            _logger.LogInformation($"RideDetails => {JsonSerializer.Serialize(rideInfoResult)}");

            var driverLogRequest = CreateDriverLogRequest(ride, "UPDATE RIDE POSITION", null, JsonSerializer.Serialize(rideInfoResult));
            await _driverLogService.Post(driverLogRequest);

            if (rideInfoResult.data.driver_name != "Ainda não informado")
            {
                rideRequest.LocationRequest = new RideLocationRequest
                {
                    Latitude = ridePositionResult.latitude,
                    Longitude = ridePositionResult.longitude
                };

                await _rideService.Put(ride.RideID, rideRequest);
            }

            if (rideRequest.RideStatusID == RideStatusFinished)
            {
                await _rideService.PostFunction(ride.RideID, new RideProviderFunctionRequest(DateTime.Now, providerId, 4));
            }
        }

        return Result.Success();
    }

    private DriverLogRequest CreateDriverLogRequest(RideResult ride, string method, int? rideStatusID = null, string observation = null)
    {
        return new DriverLogRequest
        {
            Latitude = null,
            Longitude = null,
            RideID = ride.RideID,
            RideProviderID = ride?.RideProviderID,
            ProductID = null,
            ProviderID = ride.ProviderID,
            Observation = observation ?? JsonSerializer.Serialize(ride).ToString(),
            RideStatusID = rideStatusID ?? 0,
            Method = method
        };
    }
}