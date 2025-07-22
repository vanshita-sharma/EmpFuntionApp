using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(builder =>
    {
        // Add middleware here if needed, or leave empty
    })
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
    })
    .Build();

host.Run();