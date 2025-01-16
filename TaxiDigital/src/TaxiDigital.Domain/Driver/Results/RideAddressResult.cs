namespace TaxiDigital.Domain.Driver.Results;

public sealed class RideAddressResult
{
    public int RideAddressID { get; set; }
    public string Address { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public int RideAddressTypeID { get; set; }
}
