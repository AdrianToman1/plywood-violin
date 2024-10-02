namespace PlywoodViolin.UnitTests.SteadyState
{
    public class InternalServerErrorFunctionUnitTest
    {
        private readonly ILogger<FunctionWrapper> _logger;

        public InternalServerErrorFunctionUnitTest(ITestOutputHelper output)
        {
            _logger = new XunitLogger<FunctionWrapper>(output);
        }

        [Fact]
        public void RunReturnsInternalServerError()
        {
            // Arrange
            var functionWrapper = new FunctionWrapper(_logger);
            var internalServerErrorFunction = new InternalServerErrorFunction(functionWrapper);

            var request = new DefaultHttpContext().Request;

            // Act
            var actionResult = internalServerErrorFunction.RunStatusCode(request, null, _logger);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<ContentResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.InternalServerError, ((ContentResult)actionResult.Result).StatusCode);
        }
    }
}