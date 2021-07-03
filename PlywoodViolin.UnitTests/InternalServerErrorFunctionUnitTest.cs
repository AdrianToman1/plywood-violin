using System.Net;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace PlywoodViolin.UnitTests
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
