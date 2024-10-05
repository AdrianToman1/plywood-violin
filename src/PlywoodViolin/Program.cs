using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlywoodViolin.Middleware;
using PlywoodViolin.Monkey;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(worker => { worker.UseMiddleware<ExceptionHandlerMiddleware>(); })
    .ConfigureServices(services => { services.AddSingleton<IRandom, Random>(); })
    .Build();

host.Run();