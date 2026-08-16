using JSL.Logging.Formatters;
using System.Diagnostics;

namespace JSL.Logging.Sinks;

/// <summary>
/// A sink that routes logs to the debug console
/// </summary>
public class DebugConsoleSink : ILogSink
{
    [ThreadStatic] internal static bool SuppressDebugCapture = false;

    protected virtual ILogFormatter formatter { get; set; } = DefaultFormatter.Instance;
    public virtual ILogFormatter Formatter
    {
        get => formatter;
        set
        {
            if (value is null) formatter = DefaultFormatter.Instance;
            else formatter = value;
        }
    }

    public virtual void Route(LogObject log)
    {
#if DEBUG
        string formatted = DefaultFormatter.Instance.Format(log);

        SuppressDebugCapture = true;
        Debug.WriteLine(formatted);
        SuppressDebugCapture = false;
#endif
    }

    public readonly struct DebugCaptureSuppressionScope : IDisposable
    {
        public DebugCaptureSuppressionScope() => SuppressDebugCapture = true;
        public void Dispose() => SuppressDebugCapture = false;
    }
}