using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PlywoodViolin;


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

    public static class IWebJobsRouterExtensions
    {
        public static List<Route> GetRoutes(this IWebJobsRouter router)
        {
            var type = typeof(WebJobsRouter);
            var fields = type.GetRuntimeFields();
            var field = fields.FirstOrDefault(f => f.Name == "_functionRoutes");
            var functionRoutes = field.GetValue(router);
            var routeCollection = (RouteCollection)functionRoutes;
            var routes = GetRoutes(routeCollection);
            return routes;
        }

        private static List<Route> GetRoutes(RouteCollection collection)
        {
            var routes = new List<Route>();
            for (var i = 0; i < collection.Count; i++)
            {
                var nestedCollection = collection[i] as RouteCollection;
                if (nestedCollection != null)
                {
                    routes.AddRange(GetRoutes(nestedCollection));
                    continue;
                }

                routes.Add((Route)collection[i]);
            }

            return routes;
        }
    }

    public class RoutePriorityExtensionConfigProvider : IExtensionConfigProvider
    {
        private readonly IApplicationLifetime applicationLifetime;
        private readonly IWebJobsRouter router;

        public RoutePriorityExtensionConfigProvider(IApplicationLifetime applicationLifetime, IWebJobsRouter router)
        {
            this.applicationLifetime = applicationLifetime;
            this.router = router;

            this.applicationLifetime.ApplicationStarted.Register(() => { ReorderRoutes(); });
        }

        public void Initialize(ExtensionConfigContext context)
        {
        }

        public void ReorderRoutes()
        {
            var unorderedRoutes = router.GetRoutes();
            var routePrecedence = Comparer<Route>.Create(RouteComparison);
            var orderedRoutes = unorderedRoutes.OrderBy(id => id, routePrecedence);
            var orderedCollection = new RouteCollection();
            foreach (var route in orderedRoutes)
            {
                orderedCollection.Add(route);
            }

            router.ClearRoutes();
            router.AddFunctionRoutes(orderedCollection, null);
        }

        private static int RouteComparison(Route x, Route y)
        {
            var xTemplate = x.ParsedTemplate;
            var yTemplate = y.ParsedTemplate;

            for (var i = 0; i < xTemplate.Segments.Count; i++)
            {
                if (yTemplate.Segments.Count <= i)
                {
                    return -1;
                }

                var xSegment = xTemplate.Segments[i].Parts[0];
                var ySegment = yTemplate.Segments[i].Parts[0];

                if (!xSegment.IsCatchAll && ySegment.IsCatchAll)
                {
                    return -1;
                }

                if (xSegment.IsCatchAll && !ySegment.IsCatchAll)
                {
                    return 1;
                }

                if (!xSegment.IsParameter && ySegment.IsParameter)
                {
                    return -1;
                }

                if (xSegment.IsParameter && !ySegment.IsParameter)
                {
                    return 1;
                }

                if (xSegment.IsParameter)
                {
                    if (xSegment.InlineConstraints.Count() > ySegment.InlineConstraints.Count())
                    {
                        return -1;
                    }

                    if (xSegment.InlineConstraints.Count() < ySegment.InlineConstraints.Count())
                    {
                        return 1;
                    }
                }
                else
                {
                    var comparison = string.Compare(xSegment.Text, ySegment.Text, StringComparison.OrdinalIgnoreCase);
                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }
            }

            if (yTemplate.Segments.Count > xTemplate.Segments.Count)
            {
                return 1;
            }

            return 0;
        }
    }
}
