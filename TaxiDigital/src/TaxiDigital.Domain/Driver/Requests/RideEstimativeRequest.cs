namespace TaxiDigital.Domain.Driver.Requests;

public sealed class RideEstimativeRequest
{
    public RideEstimativeRequest(string productId, decimal waitingTime, decimal estimateMin, decimal estimatemax, decimal fee)
    {
        ProductID = productId;
        WaitingTime = Convert.ToInt32(waitingTime / 60);
        Price = estimateMin + estimatemax / 2;
        Fee = fee;
    }

    public RideEstimativeRequest(decimal price, string productId, int waitingTime)
    {
        Price = price;
        ProductID = productId;
        WaitingTime = waitingTime;
    }

    public string ProductID { get; set; }
    public int WaitingTime { get; set; }
    public decimal Price { get; set; }
    public decimal Fee { get; set; }
    public string FareID { get; set; }
}
