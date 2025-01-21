using Refit;
using TaxiDigital.Domain.Authorize.Requests;
using TaxiDigital.Domain.Authorize.Results;
using TaxiDigital.Domain.Estimate.Requests;
using TaxiDigital.Domain.Estimate.Results;
using TaxiDigital.Domain.Ride.Requests;
using TaxiDigital.Domain.Ride.Results;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Infrastructure.APIs;

[Headers("Authorization: Bearer")]
internal interface ITaxiDigitalApi
{
    #region Booking

    [Post("/booking/estimate/json")]
    Task<ApiResult<EstimateResult>> EstimateTaxiDigital([Body] EstimateRequest request, [Header("Authorization")] string token);

    [Post("/booking/add/json")]
    Task<ApiResult<BookRideResult>> BookRide([Body] BookRideRequest request, [Header("Authorization")] string token);

    [Post("/booking/cancel/json")]
    Task<ApiResult<CancelRideResult>> CancelRide([Body] CancelRideRequest request, [Header("Authorization")] string token);

    [Post("/booking/details/json")]
    Task<ApiResult<RideDetailsResult>> RideDetails([Body] RideDetailsRequest request, [Header("Authorization")] string token);

    [Post("/booking/info/json")]
    Task<ApiResult<RideInfoResult>> RideInfo([Body] RideInfoRequest request, [Header("Authorization")] string token);

    #endregion

    #region User

    [Post("/user/create_authorized/json")]
    Task<ApiResult<CreateAuthorizedResult>> CreateAuthorized([Body] AuthorizedRequest request, [Header("Authorization")] string token);

    #endregion
}
