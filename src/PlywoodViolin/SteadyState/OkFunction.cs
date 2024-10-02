using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState;

public class OkFunction : AbstractSteadyStateFunction
{
    private readonly FunctionWrapper _functionWrapper;

    public OkFunction(FunctionWrapper functionWrapper)
    {
        _functionWrapper = functionWrapper ?? throw new ArgumentNullException(nameof(functionWrapper));
    }

    protected override int StatusCode => (int)HttpStatusCode.OK;

    protected override string HtmlTitle => "OK";

    [Function("OkFunction")]
    public Task<IActionResult> RunStatusReasonPhrase(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "OK")]
        HttpRequest request,
        ExecutionContext context,
        ILogger log)
    {
        return _functionWrapper.Execute(() => GetActionResult(request, context));
    }

    [Function("Ok200Function")]
    public Task<IActionResult> RunStatusCode(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "200")]
        HttpRequest request,
        ExecutionContext context,
        ILogger log)
    {
        return _functionWrapper.Execute(() => GetActionResult(request, context));
    }

    protected override object GetObjectContent()
    {
        return new { foo = "bar" };
    }
}