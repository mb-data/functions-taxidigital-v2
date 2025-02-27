namespace TaxiDigital.Domain.Ride.Results;

public sealed class RideStatusResult
{
    public string type { get; set; }
    public string code { get; set; }
    public int status { get; set; }
    public long datetime { get; set; }
    public RideStatusDataResult data { get; set; }
    public int booking_id { get; set; }
    public string external_authorization_id { get; set; }
}