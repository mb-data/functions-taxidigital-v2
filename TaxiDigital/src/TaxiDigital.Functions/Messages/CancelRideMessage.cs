namespace TaxiDigital.Functions.Messages;

internal sealed record CancelRideMessage
{
    public int RideId { get; init; }
}
