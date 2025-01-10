namespace TaxiDigital.Domain.Estimate.Requests;

public sealed class EstimativeQueueRequest
{
    public EstimativeQueueRequest(int rideID, int providerID, string companyConfigurationTypeID)
    {
        RideID = rideID;
        ProviderID = providerID;
        CompanyConfigurationTypeID = companyConfigurationTypeID;
    }

    public int RideID { get; set; }

    public int ProviderID { get; set; }

    public string CompanyConfigurationTypeID { get; set; }
}
