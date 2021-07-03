using System.Net;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace PlywoodViolin.UnitTests
{
    public class OkFunctionUnitTest
    {
        [Fact]
        public void RunReturnsOk()
        {
            var actionResult = OkFunction.Run(null, null);

            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<StatusCodeResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, ((StatusCodeResult)actionResult).StatusCode);
        }
    }
}
