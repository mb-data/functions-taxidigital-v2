using TaxiDigital.Domain.Driver.Results;

namespace TaxiDigital.Application.Services.DriverData;

public interface IProviderService
{
    Task<List<ProviderResult>> Get(int? integrationId, int? providerId);
}
