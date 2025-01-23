namespace TaxiDigital.Domain.User.Requests;

public sealed class UserProviderConfigurationRequest
{
    public UserProviderConfigurationRequest(int providerID, int userProviderConfigurationTypeID, string value)
    {
        ProviderID = providerID;
        UserProviderConfigurationTypeID = userProviderConfigurationTypeID;
        Value = value;
    }

    public int ProviderID { get; set; }
    public int UserProviderConfigurationTypeID { get; set; }
    public string Value { get; set; }
}
