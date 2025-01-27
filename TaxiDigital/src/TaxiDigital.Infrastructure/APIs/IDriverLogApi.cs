using Refit;
using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Infrastructure.APIs;

[Headers("Authorization: Bearer")]
internal interface IDriverLogApi
{
    [Post("/api/DriverLog")]
    Task<ApiResult<DriverLogResult>> PostDriverLog(DriverLogRequest request);
}
