using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.Infrastructure.APIs;

namespace TaxiDigital.Infrastructure.Services.DriverData;

internal sealed class ProviderService(IDriverDataApi driverDataApi) : IProviderService
{
    private readonly IDriverDataApi _driverDataApi = driverDataApi ?? throw new ArgumentNullException(nameof(driverDataApi));

    public async Task<List<ProviderResult>> Get(int? integrationId, int? providerId)
    {
        var response = await _driverDataApi.Get(integrationId, providerId);
        return response.Result;
    }
}
