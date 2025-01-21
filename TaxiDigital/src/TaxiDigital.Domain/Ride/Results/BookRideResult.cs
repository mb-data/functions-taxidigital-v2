namespace TaxiDigital.Domain.Ride.Results;

public sealed class BookRideResult
{
    public int status { get; set; }
    public string message { get; set; }
    public int data { get; set; }
    public int booking_code { get; set; }
}
