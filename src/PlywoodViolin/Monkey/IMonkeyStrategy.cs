using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PlywoodViolin.Monkey
{
    public interface IMonkeyStrategy
    {
        Task<IActionResult> GetActionResult();
    }
}
