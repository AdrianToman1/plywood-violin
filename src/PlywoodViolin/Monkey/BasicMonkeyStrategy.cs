using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlywoodViolin.Monkey;

public class BasicMonkeyStrategy : IMonkeyStrategy
{
    public BasicMonkeyStrategy(IRandom random)
    {
        Random = random ?? throw new ArgumentNullException(nameof(random));
    }

    public IRandom Random { get; }

    public Task<IActionResult> GetActionResult(HttpRequest request)
    {
        var randomValue = Random.GetRandomValue();

        if (randomValue > 0.5m)
        {
            return Task.FromResult<IActionResult>(new InternalServerErrorResult());
        }

        return Task.FromResult<IActionResult>(new OkResult());
    }
}