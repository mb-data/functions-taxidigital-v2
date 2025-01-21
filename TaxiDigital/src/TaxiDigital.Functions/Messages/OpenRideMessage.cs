namespace TaxiDigital.Functions.Messages;

internal sealed record OpenRideMessage
{
    public int RideId { get; init; }
}
