using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlywoodViolin.SteadyState;

namespace PlywoodViolin.Unknown;

/// <inheritdoc cref="IUnknownFunction" />
public sealed class UnknownFunction : AbstractSteadyStateFunction, IUnknownFunction
{
    protected override int StatusCode => (int)HttpStatusCode.NotFound;

    /// <inheritdoc />
    public Task<IActionResult> Run(
        HttpRequest request)
    {
        return GetActionResult(request);
    }

    //protected override Task<string> GetHtmlContent()
    //{
    //    return Task.FromResult("<html><body>This isn't what you're looking for</body></html>");
    //}

    protected override object GetObjectContent()
    {
        return new { foo = "stuff" };
    }
}