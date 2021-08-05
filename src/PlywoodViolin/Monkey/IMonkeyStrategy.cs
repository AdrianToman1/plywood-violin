using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlywoodViolin.Monkey
{
    public interface IMonkeyStrategy
    {
        Task<IActionResult> GetActionResult(HttpRequest request);
    }
}
