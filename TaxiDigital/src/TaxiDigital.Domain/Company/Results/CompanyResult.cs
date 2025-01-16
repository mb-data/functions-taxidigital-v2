namespace TaxiDigital.Domain.User.Results;

public sealed class CompanyResult
{
    public int CompanyID { get; set; }
    public string Name { get; set; }
    public string GiveName { get; set; }
    public string Document { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Active { get; set; }
}
