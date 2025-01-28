namespace TaxiDigital.Domain.Ride.Results;

public class RidePositionResult
{
    public string type { get; set; }
    public string latitude { get; set; }
    public string longitude { get; set; }
    public string speed { get; set; }
    public string angle { get; set; }
    public long datetime { get; set; }
    public string booking_id { get; set; }
    public string external_authorization_id { get; set; }
}
