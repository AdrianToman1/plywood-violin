using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace PlywoodViolin.SteadyState;

public class DefaultFunction : AbstractSteadyStateFunction
{
    protected override int StatusCode => (int)HttpStatusCode.OK;

    [Function("DefaultFunction")]
    public Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Default")]
        HttpRequest request,
        ExecutionContext context)
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