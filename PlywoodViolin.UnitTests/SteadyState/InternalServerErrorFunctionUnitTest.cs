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
            var actionResult = InternalServerErrorFunction.Run(null, null);

            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<StatusCodeResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.InternalServerError, ((StatusCodeResult)actionResult).StatusCode);
        }
    }
}
