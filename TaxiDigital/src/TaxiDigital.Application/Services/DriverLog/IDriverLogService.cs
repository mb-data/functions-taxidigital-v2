using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;

namespace TaxiDigital.Application.Services.DriverLog;

public interface IDriverLogService
{
    Task<DriverLogResult> Post(DriverLogRequest request);
}
