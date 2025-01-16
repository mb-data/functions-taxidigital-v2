using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.Infrastructure.APIs;

namespace TaxiDigital.Infrastructure.Services.DriverData;

internal sealed class RideService(IDriverDataApi driverDataApi) : IRideService
{
    private readonly IDriverDataApi _driverDataApi = driverDataApi ?? throw new ArgumentNullException(nameof(driverDataApi));

    public async Task<List<RideResult>> FilterCancelRide(DateTime StartDate, DateTime EndDate, int RideStatusId, int ProviderID)
    {
        var response = await _driverDataApi.FilterCancelRide(StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss"), RideStatusId, ProviderID);
        return response.Result;
    }

    public async Task<List<RideResult>> FilterRideStatus(int? IntegratorID, List<int> RideStatusID)
    {
        var response = await _driverDataApi.FilterRideStatus(IntegratorID, RideStatusID);
        return response.Result;
    }

    public async Task<RideResult> Get(int id)
    {
        var response = await _driverDataApi.GetById(id);
        return response.Result;
    }

    public async Task<RideEstimativeResult> GetBestEstimative(int id)
    {
        var response = await _driverDataApi.GetBestEstimative(id);
        return response.Result;
    }

    public async Task<List<RideProviderFunctionResult>> GetFilterFunctions(int Ride, int RideProviderMethodID, int ProviderID)
    {
        var response = await _driverDataApi.GetFilterFunctions(Ride, RideProviderMethodID, ProviderID, false);
        return response.Result;
    }

    public async Task<RideEstimativeResult> PostEstimative(int RideID, RideEstimativeRequest request)
    {
        var response = await _driverDataApi.PostEstimative(RideID, request);
        return response.Result;
    }

    public async Task<RideEstimativeResult> PostFunction(int Ride, RideProviderFunctionRequest request)
    {
        var response = await _driverDataApi.PostFunction(Ride, request);
        return response.Result;
    }

    public async Task<RideResult> Put(int RideID, RideRequest request)
    {
        var response = await _driverDataApi.Put(RideID, request);
        return response.Result;
    }

    public async Task<RideEstimativeResult> PutFunction(int Ride, int RideProviderFunctionID, RideProviderFunctionRequest request)
    {
        var response = await _driverDataApi.PutFunction(Ride, RideProviderFunctionID, request);
        return response.Result;
    }

    public async Task UpdateFunctionLog(int rideID, string error, int providerId)
    {
        var functions = await _driverDataApi.GetFilterFunctions(rideID, 1, providerId, false);

        if (functions.Result.Any())
        {
            foreach (var function in functions.Result)
            {
                var processed = await _driverDataApi.PutFunction(rideID, function.RideProviderFunctionID, new RideProviderFunctionRequest(DateTime.UtcNow, error));
            }
        }
    }
}
