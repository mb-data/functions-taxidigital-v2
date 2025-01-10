using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Domain.Driver.Results;

namespace TaxiDigital.Infrastructure.Services.DriverData;

internal sealed class ProviderService : IProviderService
{
    public Task<List<ProviderResult>> Get(int? integrationId, int? providerId)
    {
        throw new NotImplementedException();
    }
}
