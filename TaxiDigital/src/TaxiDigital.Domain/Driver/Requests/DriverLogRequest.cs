namespace TaxiDigital.Domain.Driver.Requests;

public sealed class DriverLogRequest
{
    public int RideID { get; set; }
    public int ProviderID { get; set; }
    public string ProductID { get; set; }
    public string RideProviderID { get; set; }
    public string Lat { get; set; }
    public string Lng { get; set; }
    public int? RideStatusID { get; set; }
    public bool? Error { get; set; }
    public string Method { get; set; }
    public string Observation { get; set; }
}
