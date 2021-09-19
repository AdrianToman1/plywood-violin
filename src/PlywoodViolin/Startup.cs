using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PlywoodViolin;
using PlywoodViolin.Monkey;
using Random = PlywoodViolin.Monkey.Random;


// Code from https://briandunnington.github.io/azure_functions_route_priority
//
// This code will apply a ASP.NET Core like precedence to the routes. This is important
// because without it the route precedence seems to solely be determined by the alphabetically
// order of the function name!
//
// The desired route precedence is:
// 1) Literal routes, such as those for specific steady state HTTP response functions, the monkey function and default function.
// 2) The generic steady function.
// 3) The catch all wildcard route for the global not found function (will be explicitly different than the not found steady state HTTP response function).
[assembly: WebJobsStartup(typeof(StartUp))]

namespace PlywoodViolin
{
    public class StartUp : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddTransient<FunctionWrapper>();
            builder.Services.AddTransient<IRandom, Random>();

            builder.AddRoutePriority();
        }
    }

    public static class WebJobsBuilderExtensions
    {
        public static IWebJobsBuilder AddRoutePriority(this IWebJobsBuilder builder)
        {
            builder.AddExtension<RoutePriorityExtensionConfigProvider>();
            return builder;
        }
    }
}
