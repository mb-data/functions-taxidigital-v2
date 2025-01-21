namespace TaxiDigital.Domain.Estimate.Results;

public sealed class EstimateFareResult
{
    public string estimate { get; set; }
    public string low_estimate { get; set; }
    public string high_estimate { get; set; }
    public string distance { get; set; }
    public string desc_route { get; set; }
    public string estimate_id { get; set; }
}
