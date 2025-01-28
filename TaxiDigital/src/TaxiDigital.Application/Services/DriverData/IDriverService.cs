using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;

namespace TaxiDigital.Application.Services.DriverData;

public interface IDriverLogService
{
    Task<DriverLogResult> Post(DriverLogRequest request);
}
