using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;
using TaxiDigital.Application.Common.Messaging;
using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Application.Services.ExpoData;
using TaxiDigital.Application.Services.TaxiDigital;
using TaxiDigital.Domain.Driver.Requests;
using TaxiDigital.Domain.Driver.Results;
using TaxiDigital.Domain.Estimate.Requests;
using TaxiDigital.Domain.Estimate.Results;
using TaxiDigital.SharedKernel;

namespace TaxiDigital.Application.UseCases.EstimateQueue;

internal sealed class EstimateQueueCommandHandler(
    ILogger<EstimateQueueCommandHandler> logger,
    IRideService rideService,
    IProviderService providerService,
    ICompanyService companyService,
    IProductService productService,
    ITaxiDigitalService taxiDigitalService) : ICommandHandler<EstimateQueueCommand>
{
    private readonly ILogger<EstimateQueueCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IRideService _rideService = rideService ?? throw new ArgumentNullException(nameof(rideService));
    private readonly IProviderService _providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));
    private readonly ICompanyService _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
    private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    private readonly ITaxiDigitalService _taxiDigitalService = taxiDigitalService ?? throw new ArgumentNullException(nameof(taxiDigitalService));

    public async Task<Result> Handle(EstimateQueueCommand request, CancellationToken cancellationToken)
    {
        try
        {
            RideResult ride = await _rideService.Get(request.RideID);
            List<ProviderResult> providers = await _providerService.Get(null, request.ProviderID);

            if (ride == null || (providers == null && providers.Any()))
            {
                _logger.LogError("Ride or Provider not found");
                return Result.Failure(new Error("RideOrProviderNotFound", "Ride or Provider not found", ErrorType.NotFound));
            }

            string companyToken = await _companyService.GetTaxiDigitalToken(ride.CompanyID, providers[0].ProviderConfigurations.Find(x => x.ProviderConfigurationTypeID == 1).Value);

            if (string.IsNullOrEmpty(companyToken))
            {
                _logger.LogError("Company Token not found");
                return Result.Failure(new Error("CompanyTokenNotFound", "Company Token not found", ErrorType.NotFound));
            }

            await _rideService.PostFunction(request.RideID, new RideProviderFunctionRequest(request.ProviderID, 1));

            List<ProductResult> providerProducts = await _productService.Filter(request.ProviderID);

            RideAddressResult startAddress = ride.RideAdresses.FirstOrDefault(x => x.RideAddressTypeID == 1);
            RideAddressResult endAddress = ride.RideAdresses.FirstOrDefault(x => x.RideAddressTypeID == 2);

            EstimateRequest estimateRequest = new EstimateRequest
            {
                init_lat = double.Parse(startAddress.Latitude, CultureInfo.InvariantCulture),
                init_lng = double.Parse(startAddress.Longitude, CultureInfo.InvariantCulture),
                end_lat = double.Parse(endAddress.Latitude, CultureInfo.InvariantCulture),
                end_lng = double.Parse(endAddress.Longitude, CultureInfo.InvariantCulture),
                reference_date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };

            _logger.LogInformation($"EstimateQueueRequest - {request.RideID}: {JsonSerializer.Serialize(estimateRequest)}");
            EstimateResult estimateResult = await _taxiDigitalService.EstimateTaxiDigital(estimateRequest, companyToken);
            _logger.LogInformation($"EstimateQueue - {request.RideID}: {JsonSerializer.Serialize(estimateResult)}");

            if (estimateResult != null && estimateResult.data.categories.Any())
            {
                foreach (CategoryResult category in estimateResult.data.categories)
                {
                    ProductResult existProduct = providerProducts.FirstOrDefault(o => o.ProductID.Equals("TAXID " + category.category_id, StringComparison.Ordinal));

                    if (existProduct == null)
                    {
                        ProductResult newProduct = await _productService.Post(new ProductRequest("TAXID " + category.category_id, request.ProviderID, 5, category.display_name));
                    }

                    RideEstimativeRequest rideEstimativeRequest = new RideEstimativeRequest(
                        Convert.ToDecimal(category.estimate, CultureInfo.InvariantCulture),
                        "TAXID " + category.category_id,
                        int.Parse(category.pickup_estimate));

                    _logger.LogInformation($"RideEstimateRequest : {JsonSerializer.Serialize(rideEstimativeRequest)}");

                    if (rideEstimativeRequest.Price != 0)
                    {
                        RideEstimativeResult rideEstimativeResult = await _rideService.PostEstimative(request.RideID, rideEstimativeRequest);
                    }
                }
            }

            await _rideService.UpdateFunctionLog(request.RideID, null, request.ProviderID);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EstimateQueueCommandHandler");
            await _rideService.UpdateFunctionLog(request.RideID, JsonSerializer.Serialize(ex).ToString(), request.ProviderID);
            return Result.Failure(new Error("EstimateQueueCommandHandler", ex.Message, ErrorType.Failure));
        }
    }
}
