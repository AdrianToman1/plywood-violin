using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlywoodViolin.HomePage;
using PlywoodViolin.Middleware;
using PlywoodViolin.Monkey;
using PlywoodViolin.Unknown;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(worker => { worker.UseMiddleware<ExceptionHandlerMiddleware>(); })
    .ConfigureServices(services =>
    {
        services.AddSingleton<IHomePageFunction, HomePageFunction>();
        services.AddSingleton<IUnknownFunction, UnknownFunction>();
        services.AddSingleton<IRandom, Random>();
    })
    .Build();

host.Run();