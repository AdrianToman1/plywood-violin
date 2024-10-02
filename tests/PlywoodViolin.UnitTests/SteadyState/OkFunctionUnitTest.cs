namespace PlywoodViolin.UnitTests.SteadyState
{
    public class OkFunctionUnitTest
    {
        private readonly ILogger<FunctionWrapper> _logger;

        public OkFunctionUnitTest(ITestOutputHelper output)
        {
            _logger = new XunitLogger<FunctionWrapper>(output);
        }

        [Fact]
        public void RunReturnsOk()
        {
            // Arrange
            var functionWrapper = new FunctionWrapper(_logger);
            var okFunction = new OkFunction(functionWrapper);

            var request = new DefaultHttpContext().Request;

            // Act
            var actionResult = okFunction.RunStatusCode(request, null, _logger);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<ContentResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, ((ContentResult)actionResult.Result).StatusCode);
        }
    }
}