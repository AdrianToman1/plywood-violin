using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;

namespace PlywoodViolin.Monkey
{
    public class MonkeyFunction
    {
        private readonly FunctionWrapper _functionWrapper;
        private readonly IRandom _random;

        public MonkeyFunction(FunctionWrapper functionWrapper, IRandom random)
        {
            _functionWrapper = functionWrapper ?? throw new ArgumentNullException(nameof(functionWrapper));
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        [Function("MonkeyFunction")]
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Monkey")]
            HttpRequest request,
            ILogger log)
        {
            return _functionWrapper.Execute(() =>
            {
                string name = request.Query["strategy"];

                IMonkeyStrategy monkeyStrategy;

                if (string.IsNullOrWhiteSpace(name) || name.Equals("basic", StringComparison.OrdinalIgnoreCase))
                {
                    monkeyStrategy = new JsonContentMonkeyStrategy(_random);
                }
                else if (name.Equals("json", StringComparison.OrdinalIgnoreCase))
                {
                    monkeyStrategy = new JsonContentMonkeyStrategy(_random);
                }
                else
                {
                    monkeyStrategy = new BasicMonkeyStrategy(_random);
                }

                return monkeyStrategy.GetActionResult(request);
            });
        }
    }
}
