using System.Net;
using System.Threading.Tasks;
using Element.Azure.Functions.Worker.Extensions.RoutePriority;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace PlywoodViolin.SteadyState;

public class DefaultFunction : AbstractSteadyStateFunction
{
    protected override int StatusCode => (int)HttpStatusCode.OK;

    [Function("DefaultFunction")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Default")]
        HttpRequest request,
        ExecutionContext context, [RoutePriority] object ignore)
    {
        return GetActionResult(request, context);
    }

    //protected override Task<string> GetHtmlContent(ExecutionContext context)
    //{
    //    return Task.FromResult("<html><body>Hello <b>world</b></body></html>");
    //}

    protected override object GetObjectContent()
    {
        return new { foo = "bar" };
    }
}