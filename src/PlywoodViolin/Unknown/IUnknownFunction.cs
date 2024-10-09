using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace PlywoodViolin.Unknown;

/// <summary>
///     A function to return a 404 response if request path does not route to any other function.
/// </summary>
/// <remarks>
///     This function is not triggered by a <see cref="HttpTriggerAttribute" />. Instead, this function be called from
///     <see cref="WildCardRouter" />. Using an interface to inject into the WildCardRouter to invert the dependency.
/// </remarks>
public interface IUnknownFunction
{
    /// <summary>
    ///     Returns a 404 response if request path does not route to any other function.
    /// </summary>
    /// <param name="request">The HTTP request object and overall HttpContext.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation containing an <see cref="IActionResult" />
    ///     representing a 404 response.
    /// </returns>
    /// <remarks>
    ///     I desire this response of this function to be explicitly different from and the steady state not found response.
    ///     Whereas receiving a not found HTTP status code from a  steady state not found function would
    ///     be the expected, this represents a genuine error. I think it's important to differentiate
    ///     between the two.
    /// </remarks>
    Task<IActionResult> Run(HttpRequest request);
}