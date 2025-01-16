using TaxiDigital.Domain.User.Results;

namespace TaxiDigital.Application.Services.ExpoData
{
    public interface ICompanyService
    {
        Task<List<CompanyResult>> GetAll();
        Task<CompanyResult> GetById(int id);
        Task<string> GetTaxiDigitalToken(int id, string CompanyConfigurationTypeID);
    }
}
