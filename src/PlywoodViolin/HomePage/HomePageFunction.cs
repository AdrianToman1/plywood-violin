using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlywoodViolin.SteadyState;

namespace PlywoodViolin.HomePage;

/// <inheritdoc cref="IHomePageFunction" />
public sealed class HomePageFunction : AbstractSteadyStateFunction, IHomePageFunction
{
    protected override int StatusCode => (int)HttpStatusCode.OK;

    /// <inheritdoc />
    public Task<IActionResult> Run(HttpRequest request)
    {
        return GetActionResult(request);
    }

    //protected override Task<string> GetHtmlContent()
    //{
    //    return Task.FromResult("<html><body>Hello <b>world</b></body></html>");
    //}

    protected override object GetObjectContent()
    {
        return new { foo = "bar" };
    }
}