namespace TaxiDigital.Domain.Driver.Results;

public sealed class RideResult
{
    public int RideID { get; set; }
    public string UserID { get; set; }
    public DateTime Schedule { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public DateTime Create { get; set; }
    public int RideStatusID { get; set; }
    public int CompanyID { get; set; }
    public string Company { get; set; }
    public int ProviderID { get; set; }
    public string RideProviderID { get; set; }
    public decimal Price { get; set; }
    public DateTime Updated { get; set; }
    public int CategoryID { get; set; }
    public int TotalUsers { get; set; }
    public string Driver { get; set; }
    public string DriverPicture { get; set; }
    public string DriverPhone { get; set; }
    public string Car { get; set; }
    public string Plate { get; set; }
    public List<RideAddressResult> RideAdresses { get; set; }
    public DateTime? OpenRideDate { get; set; }
}
