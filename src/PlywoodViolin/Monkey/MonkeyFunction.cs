using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace PlywoodViolin.Monkey;

public class MonkeyFunction(IRandom random)
{
    private readonly IRandom _random = random ?? throw new ArgumentNullException(nameof(random));

    [Function("MonkeyFunction")]
    public Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "Monkey")]
        HttpRequest request)
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
    }
}