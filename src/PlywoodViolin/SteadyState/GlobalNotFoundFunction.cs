using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState
{
    public class GlobalNotFoundFunction : AbstractSteadyStateFunction
    {
        private readonly FunctionWrapper _functionWrapper;

        public GlobalNotFoundFunction(FunctionWrapper functionWrapper)
        {
            _functionWrapper = functionWrapper ?? throw new ArgumentNullException(nameof(functionWrapper));
        }

        protected override int StatusCode => (int)HttpStatusCode.NotFound;

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="log"></param>
        /// <param name="restOfPath"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Wildcard route from: https://briandunnington.github.io/azure_functions_wildcard_routing
        ///     This overrides the existing not found response provided by the framework.
        ///     I was unable to determine how to provide a global error handler, like ASP.NET, so I resorted
        ///     to having a wildcard catch all route.
        ///     I desire this response to be explicitly different from and steady state not found response.
        ///     Where as receiving a not found HTTP status code from a  steady state not found function would
        ///     be the expected, this represents a genuine error. I think it's important to differentiate
        ///     between the two.
        /// </remarks>
        [FunctionName("GlobalNotFoundFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "{*restOfPath}")]
            HttpRequest request,
            ILogger log,
            string restOfPath)
        {
            // The route re-ordering code throws away any proxy routes (see README for more details).
            // Instead of using a proxy to route the root path to the Default function, the root path
            // will be caught by this catch all route.
            // In the case that the path is the root path then return the result of the Default Function.
            if (string.IsNullOrWhiteSpace(restOfPath) || restOfPath == "/")
            {
                var defaultFunction = new DefaultFunction(_functionWrapper);
                return defaultFunction.Run(request, log);
            }

            return _functionWrapper.Execute(() => GetActionResult(request));
        }

        protected override string GetHtmlContent()
        {
            return $"<html><body>This isn't what you're looking for</body></html>";
        }

        protected override object GetObjectContent()
        {
            return new { foo = "stuff" };
        }
    }
}
