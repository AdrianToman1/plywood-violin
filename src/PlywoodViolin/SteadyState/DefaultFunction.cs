using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState;

public class DefaultFunction : AbstractSteadyStateFunction
{
    private readonly FunctionWrapper _functionWrapper;

    public DefaultFunction(FunctionWrapper functionWrapper)
    {
        _functionWrapper = functionWrapper ?? throw new ArgumentNullException(nameof(functionWrapper));
    }

    protected override int StatusCode => (int)HttpStatusCode.OK;

    [Function("DefaultFunction")]
    public Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Default")]
        HttpRequest request,
        ExecutionContext context,
        ILogger log)
    {
        return _functionWrapper.Execute(() => GetActionResult(request, context));
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