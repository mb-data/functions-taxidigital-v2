using TaxiDigital.Application.Services.ExpoData;
using TaxiDigital.Domain.User.Results;
using TaxiDigital.Infrastructure.APIs;

namespace TaxiDigital.Infrastructure.Services.ExpoData;

internal sealed class UserService(IExpoDataApi expoDataApi) : IUserService
{
    private readonly IExpoDataApi _expoDataApi = expoDataApi ?? throw new ArgumentNullException(nameof(expoDataApi));

    public async Task<UserResult> Get(string id)
    {
        var response = await _expoDataApi.GetUser(id);
        return response.Result;
    }
}
