using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace PlywoodViolin.Middleware;

/// <summary>
/// Middleware that catches unhandled exceptions and logs them.
/// </summary>
/// <param name="logger">The <see cref="ILogger"/> to write the unhandled exceptions to.</param>
public sealed class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) : IFunctionsWorkerMiddleware
{
    /// <summary>
    /// The <see cref="ILogger"/> to write unhandled exceptions to.
    /// </summary>
    private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Uncaught exception");

            var request = await context.GetHttpRequestDataAsync();

            if (request != null)
            {
                var response = request.CreateResponse();
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;

                var errorMessage = new { Message = "An unhandled exception occurred", Exception = e.Message };
                var responseBody = JsonSerializer.Serialize(errorMessage);

                await response.WriteStringAsync(responseBody);

                context.GetInvocationResult().Value = response;
            }
        }
    }
}