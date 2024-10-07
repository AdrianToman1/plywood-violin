using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlywoodViolin.Unknown;
using Xunit;

namespace PlywoodViolin.UnitTests.HomePage;

public sealed class UnknownFunctionUnitTests
{
    [Fact]
    public async Task RunReturnsNotFound()
    {
        // Arrange
        var unknownFunctionFunction = new UnknownFunction();
        var request = new DefaultHttpContext().Request;

        // Act
        var result = await unknownFunctionFunction.Run(request, null);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.NotFound, ((ObjectResult)result).StatusCode);
    }
}