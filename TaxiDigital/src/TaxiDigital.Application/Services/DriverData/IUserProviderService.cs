using TaxiDigital.Domain.User.Requests;
using TaxiDigital.Domain.User.Results;

namespace TaxiDigital.Application.Services.DriverData;

public interface IUserProviderService
{
    Task<UserProviderConfigurationResult> Post(string UserID, UserProviderConfigurationRequest request);
}
