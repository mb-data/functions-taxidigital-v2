using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;

namespace TaxiDigital.Application.Services.DriverData;

public interface IRideService
{
    Task<List<RideResult>> FilterCancelRide(DateTime StartDate, DateTime EndDate, int RideStatusId, int ProviderID);
    Task<List<RideResult>> FilterRideStatus(int? IntegratorID, List<int> RideStatusID);
    Task<RideResult> Get(int id);
    Task<RideEstimativeResult> GetBestEstimative(int id);
    Task<List<RideProviderFunctionResult>> GetFilterFunctions(int Ride, int RideProviderMethodID, int ProviderID);    
    Task<RideEstimativeResult> PostEstimative(int RideID, RideEstimativeRequest request);
    Task<RideEstimativeResult> PostFunction(int Ride, RideProviderFunctionRequest request);
    Task<RideResult> Put(int RideID, RideRequest request);
    Task<RideEstimativeResult> PutFunction(int Ride, int RideProviderFunctionID, RideProviderFunctionRequest request);
    Task UpdateFunctionLog(int rideID, string error, int providerId);
}
