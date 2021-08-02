using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.Monkey
{
    public class MonkeyFunction
    {
        private readonly FunctionWrapper _functionWrapper;
        private readonly IRandom _random;

        protected MonkeyFunction(FunctionWrapper functionWrapper, IRandom random)
        {
            _functionWrapper = functionWrapper ?? throw new ArgumentNullException(nameof(functionWrapper));
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        [FunctionName("MonkeyFunction")]
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Monkey")]
            HttpRequest req,
            ILogger log)
        {
            return _functionWrapper.Execute(() =>
            {
                var monkeyStrategy = new BasicMonkeyStrategy(_random);

                return monkeyStrategy.GetActionResult();
            });
        }
    }
}
