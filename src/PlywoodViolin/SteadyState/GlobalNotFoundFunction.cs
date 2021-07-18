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

        protected override int StatusCode => (int) HttpStatusCode.NotFound;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="log"></param>
        /// <param name="restOfPath"></param>
        /// <returns></returns>
        /// <remarks>
        /// Wildcard route from: https://briandunnington.github.io/azure_functions_wildcard_routing
        ///
        /// This overrides the existing not found response provided by the framework.
        ///
        /// I was unable to determine how to provide a global error handler, like ASP.NET, so I resorted
        /// to having a wildcard catch all route.
        ///
        /// I desire this response to be explicitly different from and steady state not found response.
        /// Where as receiving a not found HTTP status code from a  steady state not found function would
        /// be the expected, this represents a genuine error. I think it's important to differentiate
        /// between the two.
        /// </remarks>
        [FunctionName("GlobalNotFoundFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "{*restOfPath}")]
            HttpRequest request,
            ILogger log,
            string restOfPath)
        {
            return _functionWrapper.Execute(() => GetActionResult(request));
        }

        protected override string GetHtmlContent()
        {
            return "<html><body>Hello <b>world</b></body></html>";
        }

        protected override object GetObjectContent()
        {
            return new {foo = "stuff"};
        }
    }
}
