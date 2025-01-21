namespace TaxiDigital.Domain.Driver.Results;

public sealed class ProviderResult
{
    public int ProviderID { get; set; }

    public string Description { get; set; }

    public int IntegratorID { get; set; }

    public List<ProviderConfigurationResult> ProviderConfigurations { get; set; }
}
