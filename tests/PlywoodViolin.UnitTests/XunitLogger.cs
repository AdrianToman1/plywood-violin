using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace PlywoodViolin.UnitTests;

public class XunitLogger(ITestOutputHelper output) : ILogger, IDisposable
{
    private readonly ITestOutputHelper _output = output;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        _output.WriteLine(state.ToString());
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return this;
    }
}

public class XunitLogger<T>(ITestOutputHelper output) : ILogger<T>, IDisposable
{
    private readonly ITestOutputHelper _output = output;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        _output.WriteLine(state.ToString());
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return this;
    }
}