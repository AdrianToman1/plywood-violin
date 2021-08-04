using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace PlywoodViolin.SteadyState
{
    public abstract class AbstractSteadyStateFunction
    {
        protected abstract int StatusCode { get; }

        protected string HtmlTemplate => "..\\Content\\Html\\_template.html";

        protected virtual string HtmlTitle => "";

        protected virtual string HtmlContentPath => "..\\Content\\Html\\_template.html";

        protected Task<IActionResult> GetActionResult(HttpRequest request, ExecutionContext context)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var acceptHeader = request.Headers["Accept"];

            if (acceptHeader.Count == 0 || acceptHeader.Contains("*/*") || acceptHeader.Contains("text/html"))
            {
                return GetHtmlResult(context);
            }

            if (acceptHeader.Contains("application/json"))
            {
                return Task.FromResult(GetJsonResult());
            }

            if (acceptHeader.Contains("text/xml"))
            {
                return Task.FromResult(GetXmlResult());
            }

            return GetHtmlResult(context);
        }

        protected async Task<IActionResult> GetHtmlResult(ExecutionContext context)
        {
            return new ContentResult { Content = await GetHtmlContent(context), ContentType = "text/html", StatusCode = StatusCode };
        }

        protected IActionResult GetJsonResult()
        {
            return new ObjectResult(GetObjectContent()) { StatusCode = StatusCode };
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

        protected virtual async Task<string> GetHtmlContent(ExecutionContext context)
        {
            var template = await System.IO.File.ReadAllTextAsync(System.IO.Path.Combine(context.FunctionDirectory, HtmlTemplate));

            var title = HtmlTitle?.Trim() ?? string.Empty;

            template = template.Replace("{{ title }}", title);

            return template;
        }

        protected abstract object GetObjectContent();
    }
}
