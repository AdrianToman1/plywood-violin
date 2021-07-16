using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState
{
    public class InternalServerErrorFunction : SteadyStateFunction
    {
        private readonly FunctionWrapper _functionWrapper;

        public InternalServerErrorFunction(FunctionWrapper functionWrapper)
        {
            _functionWrapper = functionWrapper ?? throw new ArgumentNullException(nameof(functionWrapper));
        }

        protected override int StatusCode => (int) HttpStatusCode.InternalServerError;

        [FunctionName("InternalServerErrorFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "InternalServerError")]
            HttpRequest request,
            ILogger log)
        {
            return _functionWrapper.Execute(() => GetActionResult(request));
        }

        protected override string GetHtmlContent()
        {
            return "<html><body>Hello <b>world</b></body></html>";
        }

        protected override object GetObjectContent()
        {
            return new {foo = "bar"};
        }
    }
}
