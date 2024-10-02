using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlywoodViolin;
using PlywoodViolin.Monkey;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddTransient<FunctionWrapper>();
        services.AddSingleton<IRandom, Random>();
    })
    .Build();

host.Run();