using Microsoft.AspNetCore.Mvc;
using System;
using System.Web.Http;

namespace PlywoodViolin.Monkey
{
    public class BasicMonkeyStrategy : IMonkeyStrategy
    {
        public BasicMonkeyStrategy()
        : this(new Random())
        {
        }

        public BasicMonkeyStrategy(IRandom random)
        {
            Random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public IRandom Random { get; private set; }

        public IActionResult GetActionResult()
        {
            var randomValue = Random.GetRandomValue();

            if (randomValue > 0.5m)
            {
                return new InternalServerErrorResult();
            }

            return new OkResult();
        }
    }
}
