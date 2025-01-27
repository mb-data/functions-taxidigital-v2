namespace TaxiDigital.Domain.Driver.Results;

public sealed class DriverLogResult
{
    public int RideID { get; set; }
    public int ProviderID { get; set; }
    public string RideProviderID { get; set; }
    public string ProductID { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public DateTime Created { get; set; }
    public string Observation { get; set; }
    public int DriverLogID { get; set; }
}
