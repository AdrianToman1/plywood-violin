using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using PlywoodViolin.HomePage;
using PlywoodViolin.Unknown;

namespace PlywoodViolin.WildCardRouter;

/// <summary>
///     The function is a catch-all for any unrecognised HTTP path, and will handle the request.
/// </summary>
/// <remarks>
///     The route path for <see cref="HttpTriggerAttribute" /> can not be set to the root path of empty string. However,
///     any wildcard route will catch any request to the root path. The function will re-route any request to the root path
///     to <see cref="IHomePageFunction" /> any requests to any other path or <see cref="IUnknownFunction" />.
/// </remarks>
public sealed class WildCardRouterFunction(IHomePageFunction homePageFunction, IUnknownFunction unknownFunction)
{
    private readonly IHomePageFunction _homePageFunction =
        homePageFunction ?? throw new ArgumentNullException(nameof(homePageFunction));

    private readonly IUnknownFunction _unknownFunction =
        unknownFunction ?? throw new ArgumentNullException(nameof(unknownFunction));

    /// <summary>
    ///     Re-routes an HTTP request for an unrecognised HTTP path to the appropriate function to be handled.
    /// </summary>
    /// <param name="request">The HTTP request object and overall HttpContext.</param>
    /// <param name="context"></param>
    /// <param name="path">The path of the HTTP request.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation containing an <see cref="IActionResult" />
    ///     representing the result of the request to an unrecognised HTTP path.
    /// </returns>
    /// <remarks>
    ///     Wildcard route from: https://briandunnington.github.io/azure_functions_wildcard_routing
    ///     This sidesteps the existing not found response provided by the framework.
    /// </remarks>
    [Function("WildCardRouterFunction")]
    public Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "{*path}")]
        HttpRequest request,
        ExecutionContext context,
        string path)
    {
        return path switch
        {
            // Root - Call the HomePage function
            null => _homePageFunction.Run(request, context),
            "" => _homePageFunction.Run(request, context),

            // Anything else - Call the Unknown function
            _ => _unknownFunction.Run(request, context)
        };
    }
}