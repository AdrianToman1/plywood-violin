using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace PlywoodViolin.SteadyState;

public class OkFunction : AbstractSteadyStateFunction
{
    protected override int StatusCode => (int)HttpStatusCode.OK;

    protected override string HtmlTitle => "OK";

    [Function("OkFunction")]
    public Task<IActionResult> RunStatusReasonPhrase(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "OK")]
        HttpRequest request,
        ExecutionContext context)
    {
        return GetActionResult(request, context);
    }

    [Function("Ok200Function")]
    public Task<IActionResult> RunStatusCode(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "200")]
        HttpRequest request,
        ExecutionContext context)
    {
        return GetActionResult(request, context);
    }

    protected override object GetObjectContent()
    {
        return new { foo = "bar" };
    }
}