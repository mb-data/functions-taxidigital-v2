namespace TaxiDigital.Domain.User.Results;

public sealed class UserResult
{
    public string UserID { get; set; }
    public int CompanyID { get; set; }
    public int CostCenterID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Registration { get; set; }
}
