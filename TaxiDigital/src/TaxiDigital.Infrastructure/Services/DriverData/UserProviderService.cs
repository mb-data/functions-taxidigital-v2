using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Domain.User.Requests;
using TaxiDigital.Domain.User.Results;
using TaxiDigital.Infrastructure.APIs;

namespace TaxiDigital.Infrastructure.Services.DriverData;

internal sealed class UserProviderService(IDriverDataApi driverDataApi) : IUserProviderService
{
    private readonly IDriverDataApi _driverDataApi = driverDataApi ?? throw new ArgumentNullException(nameof(driverDataApi));
    public async Task<UserProviderConfigurationResult> Post(string UserID, UserProviderConfigurationRequest request)
    {
        var response = await _driverDataApi.PostUserConfiguration(UserID, request);
        return response.Result;
    }
}
