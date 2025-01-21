using Refit;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Infrastructure.APIs;

[Headers("Authorization: Bearer")]
internal interface IExpoDataApi
{
    #region Company

    [Get("/api/Company/{CompanyId}/Configuration/Type/{CompanyConfigurationTypeID}")]
    Task<ApiResult<string>> GetTaxiDigitalToken(int CompanyId, string CompanyConfigurationTypeID);

    #endregion
}
