using Refit;
using TaxiDigital.Domain.User.Results;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Infrastructure.APIs;

[Headers("Authorization: Bearer")]
internal interface IExpoDataApi
{
    #region api/Company

    [Get("/api/Company/{CompanyId}/Configuration/Type/{CompanyConfigurationTypeID}")]
    Task<ApiResult<string>> GetTaxiDigitalToken(int CompanyId, string CompanyConfigurationTypeID);

    #endregion

    #region api/User

    [Get("/api/User/{userId}")]
    Task<ApiResult<UserResult>> GetUser(string userId);

    #endregion
}
