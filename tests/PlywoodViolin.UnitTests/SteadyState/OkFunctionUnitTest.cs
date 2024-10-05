using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlywoodViolin.SteadyState;
using Xunit;

namespace PlywoodViolin.UnitTests.SteadyState;

public class OkFunctionUnitTest
{
    [Fact]
    public async Task RunReturnsOk()
    {
        // Arrange
        var okFunction = new OkFunction();

        var request = new DefaultHttpContext().Request;

        // Act
        var result = await okFunction.RunStatusCode(request, null);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, ((ObjectResult)result).StatusCode);
    }
}