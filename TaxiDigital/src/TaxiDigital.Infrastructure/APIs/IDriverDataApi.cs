using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiDigital.Domain.Driver.Results;

namespace TaxiDigital.Infrastructure.APIs;

[Headers("Authorization: Bearer")]
internal interface IDriverDataApi
{
    #region api/Provider

    [Get("/api/Provider")]
    Task<List<ProviderResult>> Get([Query] int? integrationId, [Query] int? providerId);

    #endregion
}
