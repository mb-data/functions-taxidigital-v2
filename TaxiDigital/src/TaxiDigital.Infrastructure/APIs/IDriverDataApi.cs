using Refit;
using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.Domain.User.Requests;
using TaxiDigital.Domain.User.Results;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Infrastructure.APIs;

[Headers("Authorization: Bearer")]
internal interface IDriverDataApi
{
    #region api/Product

    [Get("/api/Product/Filter")]
    Task<ApiResult<List<ProductResult>>> FilterPoduct([Query] int ProviderID);

    [Post("/api/Product")]
    Task<ApiResult<ProductResult>> PostProduct([Body] ProductRequest request);

    #endregion

    #region api/Provider

    [Get("/api/Provider")]
    Task<ApiResult<List<ProviderResult>>> Get([Query] int? integrationId, [Query] int? providerId);

    #endregion

    #region api/Ride

    [Get("/api/Ride/{id}")]
    Task<ApiResult<RideResult>> GetById(int id);

    [Get("/api/Ride/{id}/Estimative/Best")]
    Task<ApiResult<RideEstimativeResult>> GetBestEstimative(int id);

    [Get("/api/Ride/{id}/Function/Filter")]
    Task<ApiResult<List<RideProviderFunctionResult>>> GetFilterFunctions(int id, [Query] int rideProviderMethodID, [Query] int providerID, [Query] bool processed);

    [Get("/api/Ride")]
    Task<ApiResult<List<RideResult>>> FilterCancelRide([Query] string StartDate, [Query] string EndDate, [Query] int RideStatusId, [Query] int ProviderID);

    [Get("/api/Ride/RideStatus")]
    Task<ApiResult<List<RideResult>>> FilterRideStatus([Query] int? IntegratorID, [Query(CollectionFormat.Multi)] List<int> RideStatusID);

    [Post("/api/Ride/{id}/Estimative")]
    Task<ApiResult<RideEstimativeResult>> PostEstimative(int id, [Body] RideEstimativeRequest request);

    [Post("/api/Ride/{id}/Function")]
    Task<ApiResult<RideEstimativeResult>> PostFunction(int id, [Body] RideProviderFunctionRequest request);

    [Put("/api/Ride/{id}")]
    Task<ApiResult<RideResult>> Put(int id, [Body] RideRequest request);

    [Put("/api/Ride/{id}/Function/{functionId}")]
    Task<ApiResult<RideEstimativeResult>> PutFunction(int id, int functionId, [Body] RideProviderFunctionRequest request);

    #endregion

    #region api/User

    [Post("/api/User/{UserID}/Configuration")]
    Task<ApiResult<UserProviderConfigurationResult>> PostUserConfiguration(string UserID, [Body] UserProviderConfigurationRequest request);

    #endregion
}
