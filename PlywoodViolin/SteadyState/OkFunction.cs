using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.SteadyState
{
    public static class OkFunction
    {
        [FunctionName("OkFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "OK")] HttpRequest req,
            ILogger log)
        {
            return new OkResult();
        }
    }
}
