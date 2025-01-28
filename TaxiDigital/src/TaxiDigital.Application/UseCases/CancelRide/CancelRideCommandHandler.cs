using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaxiDigital.Application.Common.Messaging;
using TaxiDigital.Application.Common.Storage;
using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Application.Services.ExpoData;
using TaxiDigital.Application.Services.TaxiDigital;
using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.Domain.Notifications.Requests;
using TaxiDigital.Domain.Ride.Requests;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.UseCases.CancelRide
{
    internal class CancelRideCommandHandler(
        ILogger<CancelRideCommandHandler> logger,
        IRideService rideService,
        IProviderService providerService,
        ICompanyService companyService,
        ITaxiDigitalService taxiDigitalService,
        IStorageService storageService) : ICommandHandler<CancelRideCommand>
    {
        private readonly ILogger<CancelRideCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IRideService _rideService = rideService ?? throw new ArgumentNullException(nameof(rideService));
        private readonly IProviderService _providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));
        private readonly ICompanyService _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
        private readonly ITaxiDigitalService _taxiDigitalService = taxiDigitalService ?? throw new ArgumentNullException(nameof(taxiDigitalService));
        private readonly IStorageService _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

        public async Task<Result> Handle(CancelRideCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CancelRideCommandHandler handling command for RideId: {request.RideId}");

            RideEstimativeResult rideEstimativeResult = await _rideService.GetBestEstimative(request.RideId);
            int providerId = rideEstimativeResult.Product.ProviderID;

            try
            {
                RideResult rideResult = await _rideService.Get(request.RideId);
                List<ProviderResult> providers = await _providerService.Get(null, providerId);

                if (providers[0].IntegratorID != 8)
                {
                    _logger.LogError($"Provider {providerId} does not support cancelling rides");
                    return Result.Failure(new Error("ProviderNotSupportCancelRide", $"Provider {providerId} does not support cancelling rides", ErrorType.Failure));
                }

                await _rideService.PostFunction(request.RideId, new RideProviderFunctionRequest(providerId, 3));

                string companyToken = await _companyService.GetTaxiDigitalToken(rideResult.CompanyID, providers[0].ProviderConfigurations.Find(x => x.ProviderConfigurationTypeID == 1).Value);

                if (string.IsNullOrEmpty(companyToken))
                {
                    await _rideService.UpdateFunctionLog(request.RideId, "The Táxi Digital token is not configured", providerId);
                    return Result.Failure(new Error("TokenNotFound", $"The Táxi Digital token is not configured for Provider {providerId}", ErrorType.NotFound));
                }

                CancelRideRequest cancelRideRequest = new CancelRideRequest
                {
                    booking_id = int.Parse(rideResult.RideProviderID),
                    description = "Corrida cancelada",
                    notify_user = false,
                    reason_id = 2
                };

                _logger.LogInformation($"CancelRideRequest - {rideResult.RideID}: {JsonSerializer.Serialize(cancelRideRequest)}");
                var cancelRide = await _taxiDigitalService.CancelRide(cancelRideRequest, companyToken);
                _logger.LogInformation($"CancelRide - {rideResult.RideID}: {JsonSerializer.Serialize(cancelRide)}");

                var rideUpdateResult = await _rideService.Put(request.RideId, new RideRequest(3));
                _logger.LogInformation($"RideUpdateResult : {JsonSerializer.Serialize(rideUpdateResult)}");

                await _rideService.UpdateFunctionLog(request.RideId, null, providerId);

                SendNotificationRequest sendNotificationRequest = new()
                {
                    RideStatusID = 13,
                    RideID = rideResult.RideID,
                    UserID = rideResult.UserID,
                    Origin = rideResult.RideAdresses?.Find(x => x.RideAddressTypeID == 1).Address ?? "",
                    Destination = rideResult.RideAdresses?.Find(x => x.RideAddressTypeID == 2).Address ?? "",
                    Updated = rideResult.Updated.AddHours(-3).ToString("dd/MM/yyyy HH:mm"),
                    Provider = providers[0].Description,
                    ProviderID = rideResult.ProviderID,
                    Vehicle = rideResult.Car ?? "",
                    Plate = rideResult.Plate ?? "",
                    Driver = rideResult.Driver ?? "",
                    DriverPhone = rideResult.DriverPhone ?? ""
                };

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));
                var requestBase64 = Convert.ToBase64String(plainTextBytes);

                await _storageService.AddMessage("send-driver-notification", requestBase64);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while cancelling ride {request.RideId}");
                return Result.Failure(new Error("FailureCancelRide", $"Error while cancelling ride {request.RideId}", ErrorType.Failure);
            }

            return Result.Success();
        }
    }
}
