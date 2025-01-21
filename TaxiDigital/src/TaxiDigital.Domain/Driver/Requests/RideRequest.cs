namespace TaxiDigital.Domain.Driver.Requests;

public sealed class RideRequest
{
    public RideRequest() { }

    public RideRequest(int rideStatusId, int providerId, string rideProviderId)
    {
        RideStatusID = rideStatusId;
        ProviderID = providerId;
        RideProviderID = rideProviderId;
    }

    public RideRequest(int rideStatusId)
    {
        RideStatusID = rideStatusId;
    }

    public string UserID { get; set; }
    public DateTime Schedule { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public int RideStatusID { get; set; }
    public int CompanyID { get; set; }
    public string Company { get; set; }
    public int ProviderID { get; set; }
    public string RideProviderID { get; set; }
    public decimal Price { get; set; }
    public int CategoryID { get; set; }
    public int TotalUsers { get; set; }
    public string Driver { get; set; }
    public string DriverPicture { get; set; }
    public string DriverPhone { get; set; }
    public string Car { get; set; }
    public string Plate { get; set; }
    public RideLocationRequest LocationRequest { get; set; }
}
