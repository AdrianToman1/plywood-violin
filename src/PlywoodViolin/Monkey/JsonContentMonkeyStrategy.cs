using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PlywoodViolin.Monkey
{
    public class JsonContentMonkeyStrategy : IMonkeyStrategy
    {
        public JsonContentMonkeyStrategy(IRandom random)
        {
            Random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public IRandom Random { get; }

        public Task<IActionResult> GetActionResult(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

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

            var acceptHeader = request.Headers["Accept"];

            if (acceptHeader.Count == 0 || acceptHeader.Contains("*/*") || acceptHeader.Contains("application/json"))
            {
                return Task.FromResult(GetJsonResult(200, content));
            }

            if (acceptHeader.Contains("text/html"))
            {
                return Task.FromResult(GetJsonResult(200, content));
//                return GetHtmlResult(context);
            }

            if (acceptHeader.Contains("text/xml"))
            {
                return Task.FromResult(GetXmlResult(200, content));
            }

            return Task.FromResult<IActionResult>(new OkResult());
        }


        protected IActionResult GetJsonResult(int statusCode, object content)
        {
            return new ObjectResult(content) { StatusCode = statusCode };
        }

        protected IActionResult GetXmlResult(int statusCode, object content)
        {
            // Provides an outer tag when to serialized.
            var wrapper = new
            {
                result = content
            };

            // Want to serialize an anonymous object to XML.
            // Apparently, it's not as easy as it sounds.
            // Used a solution from the following SO answer:
            // https://stackoverflow.com/a/58242299/651104
            // If there is a better way I'm all ears. 
            var doc = JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(wrapper));

            return new ContentResult { Content = doc.OuterXml, ContentType = "text/xml", StatusCode = statusCode };
        }
    }
}
