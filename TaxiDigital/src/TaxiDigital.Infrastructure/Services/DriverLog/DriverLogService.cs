using TaxiDigital.Application.Services.DriverLog;
using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.Infrastructure.APIs;

namespace TaxiDigital.Infrastructure.Services.DriverLog;

internal sealed class DriverLogService(IDriverLogApi driverLogApi) : IDriverLogService
{
    private readonly IDriverLogApi _driverLogApi = driverLogApi ?? throw new ArgumentNullException(nameof(driverLogApi));

    public async Task<DriverLogResult> Post(DriverLogRequest request)
    {
        var response = await _driverLogApi.PostDriverLog(request);
        return response.Result;
    }
}
