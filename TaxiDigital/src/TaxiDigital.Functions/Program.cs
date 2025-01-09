using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using TaxiDigital.Application;
using TaxiDigital.Infrastructure;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplication();
        services.AddInfrastructure();
    })
    .Build();

host.Run();