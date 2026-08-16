using JSL.Logging.Handlers;
using JSL.Logging.Sinks;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace JSL.Logging.Loggers;

/// <summary>
/// <see cref="TraceListener"/> to capture debug outputs and turn them into an <see cref="ILogSource"/>. You must manually add this to <see cref="Trace.Listeners"/> for it to capture anything.
/// </summary>
/// <remarks>When a partial write (<see cref="Debug.Write"/>) is captured it will be appended in a <see cref="StringBuilder"/> until the next <see cref="Debug.WriteLine"/></remarks>
public class DebugCapturer : TraceListener, ILogSource
{
    public new virtual string Name { get; } = nameof(DebugCapturer);
    public virtual string Alias { get; } = "DBGCAPTURE";
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

    protected virtual ThreadLocal<StringBuilder> partialBuilder { get; } = new(() => new StringBuilder());

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { throw new NotSupportedException($"{nameof(DebugCapturer)} is not to be logged to manually."); }
    public bool IsEnabled(LogLevel logLevel) => true;
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public override void Write(string? message)
    {
        if (message == null) return;
        partialBuilder.Value.Append(message);
    }

    public override void WriteLine(string? message)
    {
        if (DebugConsoleSink.SuppressDebugCapture) return; // prevent potential stack overflow from DebugConsoleSink

        var time = DateTime.Now;
        string? thread = Thread.CurrentThread.Name;
        var builder = partialBuilder.Value;

        if(builder.Length > 0)
        {
            string built = builder.ToString();
            builder.Clear();

            Handler!.Dump(new LogObject()
            {
                LogLevel = LogLevel.Information,
                Message = built,
                Provider = this,
                Timestamp = time,
                ThreadName = thread,
                Exception = null
            });
        }

        if (message == null) return;
        Handler!.Dump(new LogObject()
        {
            LogLevel = LogLevel.Information,
            Message = message,
            Provider = this,
            Timestamp = time,
            ThreadName = thread,
            Exception = null
        });
    }
}