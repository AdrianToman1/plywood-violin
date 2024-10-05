using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PlywoodViolin.Middleware;
using Xunit;

namespace PlywoodViolin.UnitTests.Middleware;

public class ExceptionHandlerMiddlewareUnitTests
{
    [Fact]
    public void Should_ThrowArgumentNullException_When_LoggerIsNull()
    {
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => { _ = new ExceptionHandlerMiddleware(null); });
    }

    [Fact]
    public async Task Should_LogException_When_MiddlewareThrowsException_And_NoHttpRequest()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionHandlerMiddleware>>();
        var errorHandlerMiddleware = new ExceptionHandlerMiddleware(mockLogger.Object);

        var mockInvocationResult = new Mock<InvocationResult>();
        mockInvocationResult.SetupProperty(x => x.Value);
        var invocationResult = mockInvocationResult.Object;

        // FunctionContext.GetHttpRequestDataAsync is an extension method, so we need to go a bit deeper with the mocking.
        var mockHttpRequestDataFeature = new Mock<IHttpRequestDataFeature>();
        mockHttpRequestDataFeature.Setup(x => x.GetHttpRequestDataAsync(It.IsAny<FunctionContext>()))
            .ReturnsAsync(() => null);
        var mockInvocationFeatures = new Mock<IInvocationFeatures>();
        mockInvocationFeatures.Setup(x => x.Get<IHttpRequestDataFeature>())
            .Returns(() => mockHttpRequestDataFeature.Object);
        var mockFunctionContext = new Mock<FunctionContext>();
        mockFunctionContext.Setup(x => x.Features).Returns(mockInvocationFeatures.Object);

        var exception = new Exception("Test exception");

        // Act
        await errorHandlerMiddleware.Invoke(mockFunctionContext.Object, context => throw exception);

        // Assert
        mockLogger.Verify(
            x => x.Log(LogLevel.Critical, It.IsAny<EventId>(),
                It.Is<object>(o =>
                    string.Equals("Uncaught exception", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                exception, (Func<object, Exception, string>)It.IsAny<object>()), Times.Once);
        Assert.Null(invocationResult.Value);
    }

    // Unable to find a way to mock the FunctionContext.GetInvocationResult() call to test this scenario due it being an extension method and IFunctionBindingsFeature having internal access.
    //[Fact]
    //public async Task Should_LogException_And_ReplaceHttpResponse_When_MiddlewareThrowsException_And_HttpRequest()
    //{
    //    // Arrange
    //    var mockLogger = new Mock<ILogger<ExceptionHandlerMiddleware>>();
    //    var errorHandlerMiddleware = new ExceptionHandlerMiddleware(mockLogger.Object);

    //    var mockInvocationResult = new Mock<InvocationResult>();
    //    mockInvocationResult.SetupProperty(x => x.Value);
    //    var invocationResult = mockInvocationResult.Object;

    //    // FunctionContext.GetHttpRequestDataAsync is an extension method, so we need to go a bit deeper with the mocking.
    //    var mockHttpResponseDate = new Mock<HttpResponseData>();
    //    mockHttpResponseDate.SetupProperty(x => x.StatusCode);
    //    mockHttpResponseDate.Setup(x => x.WriteStringAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
    //    var mockHttpRequestData = new Mock<HttpRequestData>();
    //    mockHttpRequestData.Setup(x => x.CreateResponse()).Returns(mockHttpResponseDate.Object);
    //    var mockHttpRequestDataFeature = new Mock<IHttpRequestDataFeature>();
    //    mockHttpRequestDataFeature.Setup(x => x.GetHttpRequestDataAsync(It.IsAny<FunctionContext>())).ReturnsAsync(() => null);
    //    var mockFunctionBindingsFeature = new Mock<IFunctionBindingsFeature>();
    //    mockFunctionBindingsFeature.Setup(x => x.InvocationResult).Returns(invocationResult);
    //    var mockInvocationFeatures = new Mock<IInvocationFeatures>();
    //    mockInvocationFeatures.Setup(x => x.Get<IHttpRequestDataFeature>()).Returns(() => mockHttpRequestDataFeature.Object);
    //    mockInvocationFeatures.Setup(x => x.Get<IFunctionBindingsFeature>()).Returns(() => mockFunctionBindingsFeature.Object);
    //    var mockFunctionContext = new Mock<FunctionContext>();
    //    mockFunctionContext.Setup(x => x.Features).Returns(mockInvocationFeatures.Object);

    //    var exception = new Exception("Test exception");

    //    // Act
    //    await errorHandlerMiddleware.Invoke(mockFunctionContext.Object, (context) => throw exception);

    //    // Assert
    //    mockLogger.Verify(
    //        x => x.Log(LogLevel.Critical, It.IsAny<EventId>(),
    //            It.Is<object>(o =>
    //                string.Equals("Uncaught exception", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
    //            exception, (Func<object, Exception, string>)It.IsAny<object>()), Times.Once);
    //    Assert.NotNull(invocationResult.Value);
    //}

    [Fact]
    public async Task Should_DoNothing_When_MiddlewareDoseNotThrowException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionHandlerMiddleware>>();
        var errorHandlerMiddleware = new ExceptionHandlerMiddleware(mockLogger.Object);

        // Act
        await errorHandlerMiddleware.Invoke(Mock.Of<FunctionContext>(), context => Task.CompletedTask);

        // Assert
        mockLogger.Verify(
            x => x.Log(LogLevel.Critical, It.IsAny<EventId>(),
                It.Is<object>(o =>
                    string.Equals("Uncaught exception", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(), (Func<object, Exception, string>)It.IsAny<object>()), Times.Never);
    }
}