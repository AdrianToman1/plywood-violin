using System.Net;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState
{
    public class InternalServerErrorFunction : SteadyStateFunction
    {
        protected override int StatusCode => (int) HttpStatusCode.InternalServerError;

        [FunctionName("InternalServerErrorFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "InternalServerError")]
            HttpRequest req,
            ILogger log)
        {
            return new InternalServerErrorResult();
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
