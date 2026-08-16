using JSL.Logging.Handlers;
using JSL.Logging.Sinks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace JSL.Logging.Loggers;

/// <summary>
/// Standard logger source
/// </summary>
public class StdLogger : ILogSource
{
    public virtual string Name { get; } = nameof(StdLogger);
    public virtual string Alias { get; } = "STD";
    public virtual ILogSink[] Sinks { get; set; } = [];

    protected ILogHandler handler = LogHandler.Instance;
    public virtual ILogHandler Handler
    {
        get => handler;
        set
        {
            if (value is null) handler = LogHandler.Instance;
            else handler = value;
        }
    }

    public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var time = DateTime.Now;
        string? thread = Thread.CurrentThread.Name;
        string message = formatter(state, exception);

        LogObject log = new()
        {
            LogLevel = logLevel,
            Message = message,
            Provider = this,
            Timestamp = time,
            Exception = exception,
            ThreadName = thread
        };
        Handler!.Dump(log);
    }
    public virtual bool IsEnabled(LogLevel logLevel) => true;
    public virtual IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}