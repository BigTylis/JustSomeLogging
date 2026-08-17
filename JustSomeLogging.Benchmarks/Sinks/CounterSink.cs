using JSL.Logging;

namespace JSL.Benchmarks.Sinks;

/// <summary>
/// ONLY SAFE FOR SINGLE CONSUMER HANDLERS
/// </summary>
public class CounterSink : ILogSink
{
    public ILogFormatter? Formatter { get; } = null;
    public ManualResetEventSlim ResetEvent { get; }
    public int Counted { get; private set; }
    private int requiredCount { get; }

    public CounterSink(ManualResetEventSlim resetEvent, int neededCount)
    {
        ResetEvent = resetEvent;
        requiredCount = neededCount;
    }

    public void Route(ILogObject log)
    {
        Counted++;
        if(Counted >= requiredCount) ResetEvent.Set();
    }
}