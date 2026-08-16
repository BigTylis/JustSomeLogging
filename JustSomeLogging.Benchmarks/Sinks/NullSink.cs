using JSL.Logging;
using JSL.Logging.Formatters;

namespace JSL.Benchmarks.Sinks;

public class NullSink : ILogSink
{
    public ILogFormatter? Formatter { get; } = null;
    public void Route(LogObject log)
    {
        string format = DefaultFormatter.Instance.Format(log);
        _ = format;
    }
}