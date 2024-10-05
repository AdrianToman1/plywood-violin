using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace PlywoodViolin.SteadyState;

public class InternalServerErrorFunction : AbstractSteadyStateFunction
{
    protected override int StatusCode => (int)HttpStatusCode.InternalServerError;

    protected override string HtmlTitle => "Internal Server Error";

    [Function("InternalServerErrorFunction")]
    public Task<IActionResult> RunStatusReasonPhrase(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "InternalServerError")]
        HttpRequest request,
        ExecutionContext context)
    {
        return GetActionResult(request, context);
    }

    [Function("InternalServerError500Function")]
    public Task<IActionResult> RunStatusCode(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "500")]
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