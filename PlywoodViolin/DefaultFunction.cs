using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin
{
    public class DefaultFunction : SteadyStateFunction
    {
        [FunctionName("DefaultFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Default")] HttpRequest req,
            ILogger log)
        {
            string responseMessage =
                "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.";

            return new OkObjectResult(responseMessage);
        }
    }
}
