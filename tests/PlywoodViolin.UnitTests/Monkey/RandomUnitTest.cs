using PlywoodViolin.Monkey;
using Xunit;

namespace PlywoodViolin.UnitTests.Monkey;

public class RandomUnitTest
{
    [Fact]
    public void InstantiateObject_OK()
    {
        // Arrange & Act
        var random = new Random();

        // Assert
        Assert.NotNull(random);
    }

    /// <remarks>
    ///     This is pretty lame. Is the value always actually in the range???
    ///     Alternative is to grab meaningful sample of random values and check they are all within the range.
    ///     Or maybe it can't be unit tested at all. Random is antithetical to unit testing, and I have gone to some trouble to
    ///     isolate the code.
    /// </remarks>
    [Fact]
    public void RunReturnsInternalServerError()
    {
        // Arrange
        var random = new Random();

        // Act
        var actual = random.GetRandomValue();

        // Assert
        Assert.InRange(actual, 0m, 1m);
    }
}