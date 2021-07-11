using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PlywoodViolin.SteadyState
{
    public abstract class SteadyStateFunction
    {
        protected abstract int StatusCode { get; }

        protected IActionResult GetActionResult(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var acceptHeader = request.Headers["Accept"];

            if (acceptHeader.Count == 0 || acceptHeader.Contains("*/*") || acceptHeader.Contains("text/html"))
            {
                return GetHtmlResult();
            }
            if (acceptHeader.Contains("application/json"))
            {
                return GetJsonResult();
            }
            if (acceptHeader.Contains("text/xml"))
            {
                return GetXmlResult();
            }

            return GetHtmlResult();
        }

        protected IActionResult GetHtmlResult()
        {
            return new ContentResult { Content = GetHtmlContent(), ContentType = "text/html", StatusCode = StatusCode };
        }

        protected IActionResult GetJsonResult()
        {
            return new ObjectResult(null) { StatusCode = StatusCode};
        }

        protected IActionResult GetXmlResult()
        {
            var obj = GetObjectContent();

            // Provides an outer tag when to serialized.
            var wrapper = new
            {
                result = obj
            };

            // Want to serialize an anonymous object to XML.
            // Apparently, it's not as easy as it sounds.
            // Used a solution from the following SO answer:
            // https://stackoverflow.com/a/58242299/651104
            // If there is a better way I'm all ears. 
            var doc = JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(wrapper));

            return new ContentResult { Content = doc.OuterXml, ContentType = "text/xml", StatusCode = StatusCode };
        }

        protected abstract string GetHtmlContent();

        protected abstract object GetObjectContent();
    }
}
