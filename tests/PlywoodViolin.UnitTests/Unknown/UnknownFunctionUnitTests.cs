using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlywoodViolin.HomePage;
using Xunit;

namespace PlywoodViolin.UnitTests.Unknown;

public sealed class HomePageFunctionUnitTests
{
    [Fact]
    public async Task RunReturnsOk()
    {
        // Arrange
        var homePageFunction = new HomePageFunction();
        var request = new DefaultHttpContext().Request;

        // Act
        var result = await homePageFunction.Run(request, null);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, ((ObjectResult)result).StatusCode);
    }
}