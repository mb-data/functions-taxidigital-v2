using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;

namespace TaxiDigital.Application.Services.DriverData;

public interface IRideService
{
    Task<RideEstimativeResult> GetBestEstimative(int id);

    Task<List<RideResult>> Get(int RideStatusID, int ProviderID);

    Task<RideResult> Get(string RideProviderID, int ProviderID);

    Task<RideResult> Get(int id);

    Task<RideResult> Put(int RideID, RideRequest request);

    Task<RideEstimativeResult> PostEstimative(int RideID, RideEstimativeRequest request);

    Task<RideEstimativeResult> PostFunction(int Ride, RideProviderFunctionRequest request);

    Task<RideEstimativeResult> PutFunction(int Ride, int RideProviderFunctionID, RideProviderFunctionRequest request);

    Task<List<RideProviderFunctionResult>> GetFilterFunctions(int Ride, int RideProviderMethodID, int ProviderID);

    Task<List<RideResult>> FilterRideStatus(int? IntegratorID, List<int> RideStatusID);

    Task UpdateFunctionLog(int rideID, string error, int providerId);
}
