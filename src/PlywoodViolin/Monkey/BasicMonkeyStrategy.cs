using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlywoodViolin.Monkey;

public class BasicMonkeyStrategy(IRandom random) : IMonkeyStrategy
{
    public IRandom Random { get; } = random ?? throw new ArgumentNullException(nameof(random));

    public Task<IActionResult> GetActionResult(HttpRequest request)
    {
        //var randomValue = Random.GetRandomValue();

        //if (randomValue > 0.5m)
        //{
        //    return Task.FromResult<IActionResult>(new InternalServerErrorResult());
        //}

        return Task.FromResult<IActionResult>(new OkResult());
    }
}