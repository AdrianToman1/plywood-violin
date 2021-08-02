using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState
{
    public class InternalServerErrorFunction : AbstractSteadyStateFunction
    {
        private readonly FunctionWrapper _functionWrapper;

        public InternalServerErrorFunction(FunctionWrapper functionWrapper)
        {
            _functionWrapper = functionWrapper ?? throw new ArgumentNullException(nameof(functionWrapper));
        }

        protected override int StatusCode => (int)HttpStatusCode.InternalServerError;

        [FunctionName("InternalServerErrorFunction")]
        public Task<IActionResult> RunStatusReasonPhrase(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "InternalServerError")]
            HttpRequest request,
            ExecutionContext context,
            ILogger log)
        {
            return _functionWrapper.Execute(() => GetActionResult(request, context));
        }

        [FunctionName("InternalServerError500Function")]
        public Task<IActionResult> RunStatusCode(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "500")]
            HttpRequest request,
            ExecutionContext context,
            ILogger log)
        {
            return _functionWrapper.Execute(() => GetActionResult(request, context));
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
}
