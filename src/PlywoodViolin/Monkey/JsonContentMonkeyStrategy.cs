using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlywoodViolin.Monkey;

public class JsonContentMonkeyStrategy(IRandom random) : IMonkeyStrategy
{
    public IRandom Random { get; } = random ?? throw new ArgumentNullException(nameof(random));

    public Task<IActionResult> GetActionResult(HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var randomValue = Random.GetRandomValue();

        object content;

        if (randomValue > 0.5m)
        {
            content = new { foo = "bar" };
        }
        else
        {
            content = new { foo = "baz" };
        }

        var acceptHeader = request.Headers.Accept;

        if (acceptHeader.Count == 0 || acceptHeader.Contains("*/*") || acceptHeader.Contains("application/json"))
        {
            return Task.FromResult(GetJsonResult(200, content));
        }

        if (acceptHeader.Contains("text/html"))
        {
            return Task.FromResult(GetJsonResult(200, content));
//                return GetHtmlResult();
        }

        //if (acceptHeader.Contains("text/xml"))
        //{
        //    return Task.FromResult(GetXmlResult(200, content));
        //}

        return Task.FromResult<IActionResult>(new OkResult());
    }


    protected static IActionResult GetJsonResult(int statusCode, object content)
    {
        return new ObjectResult(content) { StatusCode = statusCode };
    }

    //protected IActionResult GetXmlResult(int statusCode, object content)
    //{
    //    // Provides an outer tag when to serialized.
    //    var wrapper = new
    //    {
    //        result = content
    //    };

    //    // Want to serialize an anonymous object to XML.
    //    // Apparently, it's not as easy as it sounds.
    //    // Used a solution from the following SO answer:
    //    // https://stackoverflow.com/a/58242299/651104
    //    // If there is a better way I'm all ears. 
    //    var doc = JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(wrapper));

    //    return new ContentResult { Content = doc.OuterXml, ContentType = "text/xml", StatusCode = statusCode };
    //}
}