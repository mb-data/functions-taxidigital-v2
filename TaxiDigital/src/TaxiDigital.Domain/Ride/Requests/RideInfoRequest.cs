namespace TaxiDigital.Domain.Ride.Requests;

public class RideInfoRequest
{
    public RideInfoRequest(int bookingId)
    {
        booking_id = bookingId;
    }

    public int booking_id { get; set; }
}