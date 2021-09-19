using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace PlywoodViolin
{
    public static class WebJobsRouterExtensions
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
}
