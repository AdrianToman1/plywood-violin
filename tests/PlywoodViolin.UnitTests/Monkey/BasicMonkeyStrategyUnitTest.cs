using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlywoodViolin.Monkey;
using Xunit;
using Random = PlywoodViolin.Monkey.Random;

namespace PlywoodViolin.UnitTests.Monkey;

public class BasicMonkeyStrategyUnitTest
{
    [Fact]
    public void InstantiateObject_OK1()
    {
        // Arrange & Act
        var random = new Random();
        var basicMonkeyStrategy = new BasicMonkeyStrategy(random);

        // Assert
        Assert.NotNull(basicMonkeyStrategy);
    }

    [Fact]
    public void InstantiateObject_OK2()
    {
        // Arrange
        var random = new Random();

        // Act
        var basicMonkeyStrategy = new BasicMonkeyStrategy(random);

        // Assert
        Assert.NotNull(basicMonkeyStrategy);
        Assert.Same(random, basicMonkeyStrategy.Random);
    }

    [Fact]
    public void InstantiateObject_RandomNull_ThrowsException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>("random", () => new BasicMonkeyStrategy(null));
    }

    [Fact]
    public async Task GetActionResult_ReturnsOkResult()
    {
        // Arrange
        var basicMonkeyStrategy = new BasicMonkeyStrategy(new TestRandom(0));

        // Act
        var actionResult = await basicMonkeyStrategy.GetActionResult(null);

        // Assert
        Assert.IsAssignableFrom<OkResult>(actionResult);
    }

    [Fact]
    public async Task GetActionResult_ReturnsInternalServerErrorResult()
    {
        // Arrange
        var basicMonkeyStrategy = new BasicMonkeyStrategy(new TestRandom(1));

        // Act
        var actionResult = await basicMonkeyStrategy.GetActionResult(null);

        // Assert
        Assert.IsAssignableFrom<StatusCodeResult>(actionResult);
        Assert.Equal(500, ((StatusCodeResult)actionResult).StatusCode);
    }

    private class TestRandom(decimal value) : IRandom
    {
        private readonly decimal _value = value;

        public decimal GetRandomValue()
        {
            return _value;
        }
    }
}