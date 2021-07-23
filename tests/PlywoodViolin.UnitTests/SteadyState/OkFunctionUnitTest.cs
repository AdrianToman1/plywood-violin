using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlywoodViolin.SteadyState;
using Xunit;
using Xunit.Abstractions;

namespace PlywoodViolin.UnitTests.SteadyState
{
    public class OkFunctionUnitTest
    {
        private readonly ILogger _logger;

        public OkFunctionUnitTest(ITestOutputHelper output)
        {
            _logger = new XunitLogger(output);
        }

        [Fact]
        public void RunReturnsOk()
        {
            // Arrange
            var functionWrapper = new FunctionWrapper(_logger);
            var okFunction = new OkFunction(functionWrapper);

            var request = new DefaultHttpContext().Request;

            // Act
            var actionResult = okFunction.RunStatusCode(request, _logger);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<ContentResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, ((ContentResult)actionResult).StatusCode);
        }
    }
}
