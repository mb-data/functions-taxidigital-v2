namespace TaxiDigital.Domain.Driver.Results;

public sealed class ProviderConfigurationResult
{
    public int ProviderConfigurationID { get; set; }

    public int ProviderID { get; set; }

    public int ProviderConfigurationTypeID { get; set; }

    public string Value { get; set; }
}
