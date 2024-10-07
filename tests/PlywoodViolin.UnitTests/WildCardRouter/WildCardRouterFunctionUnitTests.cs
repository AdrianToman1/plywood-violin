using System;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Moq;
using PlywoodViolin.HomePage;
using PlywoodViolin.Unknown;
using PlywoodViolin.WildCardRouter;
using Xunit;

namespace PlywoodViolin.UnitTests.WildCardRouter;

public sealed class WildCardRouterFunctionUnitTests
{
    private readonly Mock<IHomePageFunction> _mockHomePageFunction = new();
    private readonly Mock<IUnknownFunction> _mockUnknownFunction = new();

    [Fact]
    public void Should_ThrowArgumentNullException_When_HomePageFunctionIsNull()
    {
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new WildCardRouterFunction(null, _mockUnknownFunction.Object);
        });
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_UnknownFunctionIsNull()
    {
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new WildCardRouterFunction(_mockHomePageFunction.Object, null);
        });
    }

    [Fact]
    public void Should_CallHomeFunction_When_PathIsNull()
    {
        // Arrange
        _mockHomePageFunction.Setup(x => x.Run(It.IsAny<HttpRequest>(), It.IsAny<ExecutionContext>()));
        var wildCardRouterFunction =
            new WildCardRouterFunction(_mockHomePageFunction.Object, _mockUnknownFunction.Object);
        var request = new DefaultHttpContext().Request;

        // Act
        wildCardRouterFunction.Run(request, null, null);

        // Assert
        _mockHomePageFunction.Verify(x => x.Run(request, null), Times.Once);
        _mockUnknownFunction.Verify(x => x.Run(It.IsAny<HttpRequest>(), It.IsAny<ExecutionContext>()), Times.Never);
    }

    [Fact]
    public void Should_CallHomeFunction_When_PathIsEmptyString()
    {
        // Arrange
        _mockHomePageFunction.Setup(x => x.Run(It.IsAny<HttpRequest>(), It.IsAny<ExecutionContext>()));
        var wildCardRouterFunction =
            new WildCardRouterFunction(_mockHomePageFunction.Object, _mockUnknownFunction.Object);
        var request = new DefaultHttpContext().Request;

        // Act
        wildCardRouterFunction.Run(request, null, string.Empty);

        // Assert
        _mockHomePageFunction.Verify(x => x.Run(request, null), Times.Once);
        _mockUnknownFunction.Verify(x => x.Run(It.IsAny<HttpRequest>(), It.IsAny<ExecutionContext>()), Times.Never);
    }

    [Fact]
    public void Should_CallUnknownFunction_When_PathIsNotNullOrEmptyString()
    {
        // Arrange
        _mockUnknownFunction.Setup(x => x.Run(It.IsAny<HttpRequest>(), It.IsAny<ExecutionContext>()));
        var wildCardRouterFunction =
            new WildCardRouterFunction(_mockHomePageFunction.Object, _mockUnknownFunction.Object);
        var request = new DefaultHttpContext().Request;

        // Act
        wildCardRouterFunction.Run(request, null, "unknown\\path");

        // Assert
        _mockUnknownFunction.Verify(x => x.Run(request, null), Times.Once);
        _mockHomePageFunction.Verify(x => x.Run(It.IsAny<HttpRequest>(), It.IsAny<ExecutionContext>()), Times.Never);
    }
}