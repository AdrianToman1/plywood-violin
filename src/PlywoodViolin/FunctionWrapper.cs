using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin;

/// <summary>
/// </summary>
/// <remarks>
///     Based on code from: https://blog.bruceleeharrison.com/2019/08/29/azure-function-filters/
///     This provides a mechanism to "globally" handle errors.
///     Azure Functions don't have any middleware, like ASP.NET core, so the is no error handling middleware.
///     Azure Functions technically have Filters but they are mark obsolete and apparently can't alter the response.
///     Of course, this isn't really a true global error handler, but it's a bit better than nothing.
///     I desire the Internal Server Error response to be explicitly different from the steady state Internal Server Error
///     response.
///     Where as receiving a Internal Server Error HTTP status code from a steady state Internal Server Error function
///     would
///     be the expected, this represents a genuine error. I think it's important to differentiate between the two.
/// </remarks>
public class FunctionWrapper
{
    private readonly ILogger _log;

    public FunctionWrapper(ILogger<FunctionWrapper> log)
    {
        _log = log;
    }

    public Task<IActionResult> Execute(Func<Task<IActionResult>> azureFunction)
    {
        try
        {
            return azureFunction();
        }
        catch (Exception e)
        {
            _log.LogError(e, "Unhandled exception");
            return Task.FromResult<IActionResult>(new ObjectResult(e.Message)
                { StatusCode = StatusCodes.Status500InternalServerError });
        }
    }
}