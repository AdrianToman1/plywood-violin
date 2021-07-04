using Microsoft.AspNetCore.Mvc;

namespace PlywoodViolin.Monkey
{
    public interface IMonkeyStrategy
    {
        IActionResult GetActionResult();
    }
}
