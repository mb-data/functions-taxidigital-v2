namespace TaxiDigital.Domain.Driver.Requests;

public class DriverLogRequest
{
    public int RideID { get; set; }
    public int ProviderID { get; set; }
    public string ProductID { get; set; }
    public string RideProviderID { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public int? RideStatusID { get; set; }
    public bool? Error { get; set; }
    public string Method { get; set; }
    public string Observation { get; set; }
}
