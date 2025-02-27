using Microsoft.Extensions.DependencyInjection;
using TaxiDigital.Application.Services.DriverData;
using TaxiDigital.Infrastructure.APIs;
using TaxiDigital.Infrastructure.Extensions;
using TaxiDigital.Infrastructure.Services.DriverData;
using Refit;
using System.Net.Http.Headers;

namespace TaxiDigital.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<AddBearerTokenHeader>();

        services.AddScoped<IProviderService, ProviderService>();

        services
                .AddRefitClient<IDriverDataApi>()
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Accept.Clear())
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")))
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Add("accept", "application/json"))
                .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("wdriverdataapi")))
                .AddHttpMessageHandler<AddBearerTokenHeader>();

        services
                .AddRefitClient<IExpoDataApi>()
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Accept.Clear())
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")))
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Add("accept", "application/json"))
                .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("wexpodataapi")))
                .AddHttpMessageHandler<AddBearerTokenHeader>();

        services
                .AddRefitClient<ITaxiDigitalApi>()
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Accept.Clear())
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")))
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Add("accept", "application/json"))
                .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("TaxiDigitalURL")));

        services
                .AddRefitClient<IDriverLogApi>()
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Accept.Clear())
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")))
                .ConfigureHttpClient(client => client.DefaultRequestHeaders.Add("accept", "application/json"))
                .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("wlogapi")))
                .AddHttpMessageHandler<AddBearerTokenHeader>();

        return services;
    }
}
