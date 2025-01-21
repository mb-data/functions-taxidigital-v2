namespace TaxiDigital.Domain.Estimate.Results;

public sealed class CategoryResult
{
    public string category_id { get; set; }
    public string display_name { get; set; }
    public int capacity { get; set; }
    public string pickup_estimate { get; set; }
    public double estimate { get; set; }
    public double high_estimate { get; set; }
    public double low_estimate { get; set; }
    public string fixed_price { get; set; }
    public string currency_code { get; set; }
    public string distance { get; set; }
    public string estimate_id { get; set; }
}
