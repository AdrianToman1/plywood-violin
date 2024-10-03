using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlywoodViolin.SteadyState;
using Xunit;
using Xunit.Abstractions;

namespace PlywoodViolin.UnitTests.SteadyState;

public class OkFunctionUnitTest(ITestOutputHelper output)
{
    private readonly ILogger<FunctionWrapper> _logger = new XunitLogger<FunctionWrapper>(output);

    [Fact]
    public async Task RunReturnsOk()
    {
        // Arrange
        var functionWrapper = new FunctionWrapper(_logger);
        var okFunction = new OkFunction(functionWrapper);

        var request = new DefaultHttpContext().Request;

        // Act
        var result = await okFunction.RunStatusCode(request, null);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, ((ObjectResult)result).StatusCode);
    }
}