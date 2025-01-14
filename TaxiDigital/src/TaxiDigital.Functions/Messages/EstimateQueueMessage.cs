using TaxiDigital.Domain.Estimate.Requests;

namespace TaxiDigital.Functions.Messages;

internal sealed record EstimateQueueMessage
{
    public List<EstimativeQueueRequest> EstimativeQueueRequests { get; init; }
}
