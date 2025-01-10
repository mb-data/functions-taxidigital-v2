using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiDigital.Application.Common.Messaging;
using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Domain.Estimate.Requests;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.UseCases.Estimate;

internal sealed class EstimateCommandHandler(
    ILogger<EstimateCommandHandler> logger,
    IProviderService providerService) : ICommandHandler<EstimateCommand, List<EstimativeQueueRequest>>
{
    private readonly ILogger<EstimateCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IProviderService _providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));

    public async Task<Result<List<EstimativeQueueRequest>>> Handle(EstimateCommand request, CancellationToken cancellationToken)
    {
        var providers = await _providerService.Get(8, null);

        List<EstimativeQueueRequest> estimativeQueueRequests = new();

        foreach (var provider in providers)
        {
            var providerConfig = provider.ProviderConfigurations.Find(o => o.ProviderConfigurationTypeID == 1);
            if (providerConfig != null)
            {
                estimativeQueueRequests.Add(new EstimativeQueueRequest(request.RideId, provider.ProviderID, providerConfig.Value));
            }
        }

        return Result.Success(estimativeQueueRequests);
    }
}
