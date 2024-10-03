using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlywoodViolin.SteadyState;
using Xunit;
using Xunit.Abstractions;

namespace PlywoodViolin.UnitTests.SteadyState;

public class InternalServerErrorFunctionUnitTest(ITestOutputHelper output)
{
    private readonly ILogger<FunctionWrapper> _logger = new XunitLogger<FunctionWrapper>(output);

    [Fact]
    public async Task RunReturnsInternalServerError()
    {
        // Arrange
        var functionWrapper = new FunctionWrapper(_logger);
        var internalServerErrorFunction = new InternalServerErrorFunction(functionWrapper);

        var request = new DefaultHttpContext().Request;

        // Act
        var result = await internalServerErrorFunction.RunStatusCode(request, null);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.InternalServerError, ((ObjectResult)result).StatusCode);
    }
}