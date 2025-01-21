namespace TaxiDigital.Domain.Ride.Requests;

public sealed class CancelRideRequest
{
    public int booking_id { get; set; }
    public int reason_id { get; set; }
    public string description { get; set; }
    public bool notify_user { get; set; }
}
