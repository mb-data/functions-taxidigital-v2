namespace TaxiDigital.Domain.Driver.Results;

public sealed class RideProviderFunctionResult
{
    public int RideProviderFunctionID { get; set; }
    public int ProviderID { get; set; }
    public int RideProviderMethodID { get; set; }
    public int RideID { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Processed { get; set; }
    public string Error { get; set; }
}
