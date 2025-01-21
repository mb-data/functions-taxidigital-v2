using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.Infrastructure.APIs;

namespace TaxiDigital.Infrastructure.Services.DriverData;

internal sealed class ProductService(IDriverDataApi driverDataApi) : IProductService
{
    private readonly IDriverDataApi _driverDataApi = driverDataApi ?? throw new ArgumentNullException(nameof(driverDataApi));

    public async Task<List<ProductResult>> Filter(int ProviderID)
    {
        var response = await _driverDataApi.FilterPoduct(ProviderID);
        return response.Result;
    }

    public async Task<ProductResult> Post(ProductRequest request)
    {
        var response = await _driverDataApi.PostProduct(request);
        return response.Result;
    }
}
