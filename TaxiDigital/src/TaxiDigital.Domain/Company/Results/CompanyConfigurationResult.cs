namespace TaxiDigital.Domain.User.Results;

public sealed class CompanyConfigurationResult
{
    public int CompanyID { get; set; }
    public int CompanyConfigurationTypeID { get; set; }
    public string Value { get; set; }
}
