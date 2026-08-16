using BenchmarkDotNet.Attributes;
using JSL.Benchmarks.Sinks;
using JSL.Logging;
using JSL.Logging.Loggers;
using Microsoft.Extensions.Logging;

namespace JSL.Benchmarks;

[MemoryDiagnoser]
[ThreadingDiagnoser]
[DisassemblyDiagnoser(printSource: true)]
public class GeneralBenchmarks
{
    // ----- Source Side ----- //
    private StdLogger SourceSide_source;
    [GlobalSetup(Targets = [nameof(Log_SourceSide_Full), nameof(Log_SourceSide_RawLogCall)])]
    public void Setup_SourceSide()
    {
        SourceSide_source = new()
        {
            Sinks = [new NullSink()]
        };
    }

    // ----- Sink Side ----- //
    private ILogSource SinkSide_source;
    private ILogSink SinkSide_sink;
    private LogObject SinkSide_PremadeLog;
    [GlobalSetup(Targets = [nameof(Log_SinkSide)])]
    public void Setup_SinkSide()
    {
        SinkSide_source = new StdLogger();
        SinkSide_sink = new NullSink();
        SinkSide_PremadeLog = new()
        {
            LogLevel = LogLevel.Information,
            Message = "",
            Provider = SinkSide_source,
            Timestamp = DateTime.Now,
            ThreadName = "mythread",
            Exception = null
        };
    }

    // ----- Round Trip ----- //
    private ManualResetEventSlim RoundTrip_resetEvent;
    private ILogSource RoundTrip_source;
    private ILogSink RoundTrip_sink;
    [IterationSetup(Targets = [nameof(Log_RoundTrip)])]
    public void Setup_RoundTrip()
    {
        RoundTrip_resetEvent = new();
        RoundTrip_sink = new CounterSink(RoundTrip_resetEvent, 1);
        RoundTrip_source = new StdLogger() { Sinks = [RoundTrip_sink] };
    }
    [IterationCleanup(Targets = [nameof(Log_RoundTrip)])]
    public void Cleanup_RoundTrip()
    {
        RoundTrip_resetEvent.Dispose();
    }

    private ManualResetEventSlim HighThroughput_resetEvent;
    private ILogSource HighThroughput_source;
    private ILogSink HighThroughput_sink;
    [IterationSetup(Targets = [nameof(Log_HighThroughput)])]
    public void Setup_HighThroughput()
    {
        HighThroughput_resetEvent = new();
        HighThroughput_sink = new CounterSink(HighThroughput_resetEvent, 50000);
        HighThroughput_source = new StdLogger() { Sinks = [HighThroughput_sink] };
    }
    [IterationCleanup(Targets = [nameof(Log_HighThroughput)])]
    public void Cleanup_HighThroughput()
    {
        HighThroughput_resetEvent.Dispose();
    }

    //
    //

    [Benchmark(Baseline = true)]
    public void Empty() { }

    [Benchmark]
    public void Log_SourceSide_Full()
    {
        SourceSide_source.LogInformation("hello world!");
    }

    [Benchmark]
    public void Log_SourceSide_RawLogCall()
    {
        SourceSide_source.Log<object?>(LogLevel.Information, default, null, null, (state, ex) => "");
    }

    [Benchmark]
    public void Log_SinkSide()
    {
        SinkSide_sink.Route(SinkSide_PremadeLog);
    }

    [Benchmark]
    public void Log_RoundTrip()
    {
        RoundTrip_source.LogInformation("hello world!");
        RoundTrip_resetEvent.Wait();
    }

    [Benchmark(OperationsPerInvoke = 50000)]
    public void Log_HighThroughput()
    {
        for(int i = 0; i < 50000; i++)
        {
            HighThroughput_source.LogInformation("hello world!");
        }
        HighThroughput_resetEvent.Wait();
    }
}