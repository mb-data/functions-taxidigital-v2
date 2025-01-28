using TaxiDigital.Application.Services.TaxiDigital;
using TaxiDigital.Domain.Authorize.Requests;
using TaxiDigital.Domain.Authorize.Results;
using TaxiDigital.Domain.Estimate.Requests;
using TaxiDigital.Domain.Estimate.Results;
using TaxiDigital.Domain.Ride.Requests;
using TaxiDigital.Domain.Ride.Results;
using TaxiDigital.Infrastructure.APIs;

namespace TaxiDigital.Infrastructure.Services.TaxiDigital;

internal sealed class TaxiDigitalService(ITaxiDigitalApi taxiDigitalApi) : ITaxiDigitalService
{
    private readonly ITaxiDigitalApi _taxiDigitalApi = taxiDigitalApi ?? throw new ArgumentNullException(nameof(taxiDigitalApi));

    public async Task<BookRideResult> BookRide(BookRideRequest request, string TaxiDigitalToken)
    {
        var response = await _taxiDigitalApi.BookRide(request, TaxiDigitalToken);
        return response.Result;
    }

    public async Task<CancelRideResult> CancelRide(CancelRideRequest request, string TaxiDigitalToken)
    {
        var response = await _taxiDigitalApi.CancelRide(request, TaxiDigitalToken);
        return response.Result;
    }

    public async Task<CreateAuthorizedResult> CreateAuthorized(AuthorizedRequest request, string TaxiDigitalToken)
    {
        var response = await _taxiDigitalApi.CreateAuthorized(request, TaxiDigitalToken);
        return response.Result;
    }

    public async Task<EstimateResult> EstimateTaxiDigital(EstimateRequest request, string TaxiDigitalToken)
    {
        var response = await _taxiDigitalApi.EstimateTaxiDigital(request, TaxiDigitalToken);
        return response.Result;
    }

    public async Task<RideDetailsResult> RideDetails(RideDetailsRequest request, string TaxiDigitalToken)
    {
        var response = await _taxiDigitalApi.RideDetails(request, TaxiDigitalToken);
        return response.Result;
    }

    public async Task<RideInfoResult> RideInfo(RideInfoRequest request, string TaxiDigitalToken)
    {
        var response = await _taxiDigitalApi.RideInfo(request, TaxiDigitalToken);
        return response.Result;
    }
}
