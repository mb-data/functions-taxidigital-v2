namespace TaxiDigital.Domain.User.Results;

public sealed class UserProviderConfigurationResult
{
    public int UserProviderConfigurationID { get; set; }
    public string UserID { get; set; }
    public int ProviderID { get; set; }
    public int UserProviderConfigurationTypeID { get; set; }
    public string Value { get; set; }
}