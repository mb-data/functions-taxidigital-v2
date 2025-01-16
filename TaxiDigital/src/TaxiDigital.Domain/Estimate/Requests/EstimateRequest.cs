namespace TaxiDigital.Domain.Estimate.Requests;

public sealed class EstimateRequest
{
    public double init_lat { get; set; }
    public double init_lng { get; set; }
    public double end_lat { get; set; }
    public double end_lng { get; set; }

    //Optionals
    public string reference_date { get; set; }
    public string route_type { get; set; }
    public int? distance { get; set; }
    public int? ride_duration { get; set; }
    public string encoded_polyline { get; set; }
}
