using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaxiDigital.Application.Common.Messaging;
using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.UseCases.OpenRide;

internal sealed class OpenRideCommandHandler(
    ILogger<OpenRideCommandHandler> logger,
    IRideService rideService,
    IProviderService providerService) : ICommandHandler<OpenRideCommand>
{
    private readonly ILogger<OpenRideCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IRideService _rideService = rideService ?? throw new ArgumentNullException(nameof(rideService));
    private readonly IProviderService _providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));

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

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EstimateQueueCommandHandler");
            await _rideService.UpdateFunctionLog(request.RideId, JsonSerializer.Serialize(ex).ToString(), providerId);

            return Result.Failure(new Error("EstimateQueueCommandHandler", ex.Message, ErrorType.Failure));
        }
    }
}
