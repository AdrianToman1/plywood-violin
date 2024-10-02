using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState;

/// <summary>
/// </summary>
/// <remarks>
///     A catch all function that provides a generic response for any valid HTTP Status Code that hasn't got a specific
///     function.
/// </remarks>
public class GenericSteadyStateFunction : AbstractSteadyStateFunction
{
    private readonly FunctionWrapper _functionWrapper;

    private int _statusCode;

    public GenericSteadyStateFunction(FunctionWrapper functionWrapper)
    {
        _functionWrapper = functionWrapper ?? throw new ArgumentNullException(nameof(functionWrapper));
    }

    protected override int StatusCode => _statusCode;

    public void SetStatusCode(int statusCode)
    {
        _statusCode = statusCode;
    }

    [Function("SteadyStateFunction")]
    public Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "{httpStatus}")]
        HttpRequest request,
        ExecutionContext context,
        ILogger log,
        string httpStatus)
    {
        return _functionWrapper.Execute(() =>
        {
            // Convert the name or numeric value of a HttpStatusCode enumerated constant to an
            // equivalent enumerated HttpStatusCode object, if it exists. Otherwise return the
            // global not found function result.
            if (Enum.TryParse<HttpStatusCode>(httpStatus, true, out var matchingHttpStatusCode))
            {
                SetStatusCode((int)matchingHttpStatusCode);
                return GetActionResult(request, context);
            }

            var globalNotFoundFunction = new GlobalNotFoundFunction(_functionWrapper);
            return globalNotFoundFunction.Run(request, context, log, httpStatus);
        });
    }

    protected override Task<string> GetHtmlContent(ExecutionContext context)
    {
        return Task.FromResult("<html><body>Hello <b>world</b></body></html>");
    }

    protected override object GetObjectContent()
    {
        return new { foo = "bar" };
    }
}