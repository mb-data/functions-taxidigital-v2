using TaxiDigital.Application.Services.ExpoData;
using TaxiDigital.Infrastructure.APIs;

namespace TaxiDigital.Infrastructure.Services.ExpoData;

internal sealed class CompanyService(IExpoDataApi expoDataApi) : ICompanyService
{
    private readonly IExpoDataApi _expoDataApi = expoDataApi ?? throw new ArgumentNullException(nameof(expoDataApi));

    public async Task<string> GetTaxiDigitalToken(int id, string CompanyConfigurationTypeID)
    {
        var response = await _expoDataApi.GetTaxiDigitalToken(id, CompanyConfigurationTypeID);
        return response.Result;
    }
}
