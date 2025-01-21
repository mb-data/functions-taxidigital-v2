namespace TaxiDigital.Domain.Ride.Results;

public sealed class CancelRideResult
{
    public int status { get; set; }
    public string code { get; set; }
    public string message { get; set; }
}
