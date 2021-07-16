using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.Monkey
{
    public class MonkeyFunction
    {
        private readonly FunctionWrapper _functionWrapper;

        protected MonkeyFunction(FunctionWrapper functionWrapper)
        {
            _functionWrapper = functionWrapper ?? throw new ArgumentNullException(nameof(functionWrapper));
        }

        [FunctionName("MonkeyFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Monkey")] HttpRequest req,
            ILogger log)
        {
            return _functionWrapper.Execute(() =>
            {
                var monkeyStrategy = new BasicMonkeyStrategy();

                return monkeyStrategy.GetActionResult();
            });
        }
    }
}
