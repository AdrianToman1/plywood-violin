using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace PlywoodViolin.HomePage;

/// <summary>
///     A function to return the Plywood Violin home page.
/// </summary>
/// <remarks>
///     <see cref="HttpTriggerAttribute" /> will not route to the root path. Instead, this function be called from
///     <see cref="WildCardRouter" />. Using an interface to inject into the WildCardRouter to invert the dependency.
/// </remarks>
public interface IHomePageFunction
{
    /// <summary>
    ///     Returns the Plywood Violin home page.
    /// </summary>
    /// <param name="request">The HTTP request object and overall HttpContext.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation containing an <see cref="IActionResult" />
    ///     representing the Plywood Violin home page.
    /// </returns>
    Task<IActionResult> Run(HttpRequest request);
}