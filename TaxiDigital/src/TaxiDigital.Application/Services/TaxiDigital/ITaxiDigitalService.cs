using TaxiDigital.Domain.Authorize.Requests;
using TaxiDigital.Domain.Authorize.Results;
using TaxiDigital.Domain.Estimate.Requests;
using TaxiDigital.Domain.Estimate.Results;
using TaxiDigital.Domain.Ride.Requests;
using TaxiDigital.Domain.Ride.Results;

namespace TaxiDigital.Application.Services.TaxiDigital;

public interface ITaxiDigitalService
{
    Task<EstimateResult> EstimateTaxiDigital(EstimateRequest request, string TaxiDigitalToken);
    Task<CreateAuthorizedResult> CreateAuthorized(AuthorizedRequest request, string TaxiDigitalToken);
    Task<BookRideResult> BookRide(BookRideRequest request, string TaxiDigitalToken);
    Task<CancelRideResult> CancelRide(CancelRideRequest request, string TaxiDigitalToken);
    Task<RideDetailsResult> RideDetails(RideDetailsRequest request, string TaxiDigitalToken);
    Task<RideInfoResult> RideInfo(RideInfoRequest request, string TaxiDigitalToken);
}
