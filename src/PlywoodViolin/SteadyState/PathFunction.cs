using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace PlywoodViolin.SteadyState;

// Experimental function to solutions to resolving a path to HTML template in both production and dev.
public class PathFunction : AbstractSteadyStateFunction
{
    protected override int StatusCode => (int)HttpStatusCode.OK;

    private FunctionContext context;

    [Function("PathFunction")]
    public Task<IActionResult> RunStatusReasonPhrase(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Path")]
        HttpRequest request,
        FunctionContext context)
    {
        this.context = context;

        return GetActionResult(request);
    }

    protected override object GetObjectContent()
    {
        var environmentHomeLocalRoot = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
        var environmentHome = Environment.GetEnvironmentVariable("HOME");
        var contextPathToAssembly = this.context.FunctionDefinition.PathToAssembly;
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        return new { environmentHomeLocalRoot, environmentHome, assemblyLocation, contextPathToAssembly };
    }
}