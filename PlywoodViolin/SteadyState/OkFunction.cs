using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState
{
    public class OkFunction
    {
        [FunctionName("OkFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "OK")] HttpRequest req,
            ILogger log)
        {
            return new OkResult();
        }
    }
}
