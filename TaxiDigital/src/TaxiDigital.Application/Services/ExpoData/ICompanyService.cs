namespace TaxiDigital.Application.Services.ExpoData;

public interface ICompanyService
{
    Task<string> GetTaxiDigitalToken(int id, string CompanyConfigurationTypeID);
}
