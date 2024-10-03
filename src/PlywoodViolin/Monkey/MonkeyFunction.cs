using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.Monkey;

public class MonkeyFunction(FunctionWrapper functionWrapper, IRandom random)
{
    private readonly FunctionWrapper _functionWrapper = functionWrapper ?? throw new ArgumentNullException(nameof(functionWrapper));
    private readonly IRandom _random = random ?? throw new ArgumentNullException(nameof(random));

    [Function("MonkeyFunction")]
    public Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Monkey")]
        HttpRequest request)
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