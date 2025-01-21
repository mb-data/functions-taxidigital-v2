using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;

namespace TaxiDigital.Application.Services.DriverData;

public interface IProductService
{
    Task<List<ProductResult>> Filter(int ProviderID);
    Task<ProductResult> Post(ProductRequest request);
}
