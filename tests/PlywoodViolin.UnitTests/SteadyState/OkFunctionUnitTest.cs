using System.Net;
using Microsoft.AspNetCore.Mvc;
using PlywoodViolin.SteadyState;
using Xunit;

namespace PlywoodViolin.UnitTests.SteadyState
{
    public class OkFunctionUnitTest
    {
        [Fact]
        public void RunReturnsOk()
        {
            // Arrange
            var okFunction = new OkFunction(null);

            // Act
            var actionResult = okFunction.Run(null, null);

            // Arrange
            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<StatusCodeResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, ((StatusCodeResult)actionResult).StatusCode);
        }
    }
}
