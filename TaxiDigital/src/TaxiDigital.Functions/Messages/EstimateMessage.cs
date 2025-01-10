namespace TaxiDigital.Functions.Messages;

internal sealed record EstimateMessage
{
    public int RideId { get; init; }
}
