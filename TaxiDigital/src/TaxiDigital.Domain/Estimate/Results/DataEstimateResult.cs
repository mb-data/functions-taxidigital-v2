namespace TaxiDigital.Domain.Estimate.Results;

public sealed class DataEstimateResult
{
    public int total_categories { get; set; }
    public List<CategoryResult> categories { get; set; }
    public EstimateFareResult estimate_fare { get; set; }
}
