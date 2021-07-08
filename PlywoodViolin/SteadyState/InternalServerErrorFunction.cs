using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState
{
    public class InternalServerErrorFunction : SteadyStateFunction
    {
        [FunctionName("InternalServerErrorFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "InternalServerError")] HttpRequest req,
            ILogger log)
        {
            return new InternalServerErrorResult();
        }
    }
}
