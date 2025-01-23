using TaxiDigital.Domain.User.Results;

namespace TaxiDigital.Application.Services.ExpoData;

public interface IUserService
{
    Task<UserResult> Get(string id);
}
