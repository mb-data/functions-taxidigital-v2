namespace TaxiDigital.Functions.Messages;

internal sealed record UpdateRideMessage
{
    public string Type { get; init; }
    public int BookingId { get; init; }
    public string ExternalAuthorizationId { get; init; }
}
