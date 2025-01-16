namespace TaxiDigital.Domain.Driver.Requests;

public sealed class RideProviderFunctionRequest
{
    public RideProviderFunctionRequest(int providerId, int rideProviderMethodId)
    {
        ProviderID = providerId;
        RideProviderMethodID = rideProviderMethodId;
    }

    public RideProviderFunctionRequest(DateTime created, int providerId, int rideProviderMethodId)
    {
        Created = created;
        ProviderID = providerId;
        RideProviderMethodID = rideProviderMethodId;
    }

    public int ProviderID { get; set; }
    public int RideProviderMethodID { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Processed { get; set; }
    public string Error { get; set; }
}
