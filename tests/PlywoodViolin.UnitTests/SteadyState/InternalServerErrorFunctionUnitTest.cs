using System.Net;
using Microsoft.AspNetCore.Mvc;
using PlywoodViolin.SteadyState;
using Xunit;

namespace PlywoodViolin.UnitTests.SteadyState
{
    public class InternalServerErrorFunctionUnitTest
    {
        [Fact]
        public void RunReturnsInternalServerError()
        {
            // Arrange
            var internalServerErrorFunction = new InternalServerErrorFunction(null);

            // Act
            var actionResult = internalServerErrorFunction.Run(null, null);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<StatusCodeResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.InternalServerError, ((StatusCodeResult)actionResult).StatusCode);
        }
    }
}
