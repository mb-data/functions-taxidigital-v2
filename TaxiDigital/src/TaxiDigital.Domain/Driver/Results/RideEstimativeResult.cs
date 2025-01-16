namespace TaxiDigital.Domain.Driver.Results;

public sealed class RideEstimativeResult
{
    public int RideEstimativeID { get; set; }
    public int RideID { get; set; }
    public string ProductID { get; set; }
    public int WaitingTime { get; set; }
    public decimal Price { get; set; }
    public decimal Fee { get; set; }
    public string FareID { get; set; }
}
