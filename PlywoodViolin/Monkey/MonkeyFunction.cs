using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.Monkey
{
    public static class MonkeyFunction
    {
        [FunctionName("MonkeyFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Monkey")] HttpRequest req,
            ILogger log)
        {
            var monkeyStrategy = new BasicMonkeyStrategy();

            return monkeyStrategy.GetActionResult();
        }
    }
}
