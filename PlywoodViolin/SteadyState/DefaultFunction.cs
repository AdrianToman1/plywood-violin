using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState
{
    public class DefaultFunction : SteadyStateFunction
    {
        protected override int StatusCode => (int) HttpStatusCode.OK;

        [FunctionName("DefaultFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Default")]
            HttpRequest request,
            ILogger log)
        {
            return GetActionResult(request);
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
