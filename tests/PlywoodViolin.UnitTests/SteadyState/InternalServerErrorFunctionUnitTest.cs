using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlywoodViolin.SteadyState;
using Xunit;

namespace PlywoodViolin.UnitTests.SteadyState;

public class InternalServerErrorFunctionUnitTest
{
    [Fact]
    public async Task RunReturnsInternalServerError()
    {
        // Arrange
        var internalServerErrorFunction = new InternalServerErrorFunction();

        var request = new DefaultHttpContext().Request;

        // Act
        var result = await internalServerErrorFunction.RunStatusCode(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.InternalServerError, ((ObjectResult)result).StatusCode);
    }
}