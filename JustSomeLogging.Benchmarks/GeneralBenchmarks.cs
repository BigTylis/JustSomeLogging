using BenchmarkDotNet.Attributes;
using JSL.Benchmarks.Sinks;
using JSL.Logging;
using JSL.Logging.Handlers;
using JSL.Logging.Loggers;
using Microsoft.Extensions.Logging;
using JSL.Extensions;

namespace JSL.Benchmarks;

[MemoryDiagnoser]
[ThreadingDiagnoser]
[DisassemblyDiagnoser(printSource: true)]
public class GeneralBenchmarks
{
    private ILogSource Benchmark_Log_EnqueueOnly_Source = null;
    private LogHandler Benchmark_Log_EnqueueOnly_Handler = null;
    [GlobalSetup(Targets = [nameof(Log_EnqueueOnly)])]
    public void Log_EnqueueOnly_Setup()
    {
        Benchmark_Log_EnqueueOnly_Handler = new();
        Benchmark_Log_EnqueueOnly_Source = new StdLogger()
        {
            Handler = Benchmark_Log_EnqueueOnly_Handler
        };
    }
    [GlobalCleanup(Targets = [nameof(Log_EnqueueOnly)])]
    public void Log_EnqueueOnly_Cleanup()
    {
        Benchmark_Log_EnqueueOnly_Handler.Dispose();
    }


    private LogHandler Benchmark_Log_EndToEnd_Throughput_Handler = null;
    private ILogSource Benchmark_Log_EndToEnd_Throughput_Source = null;
    private ManualResetEventSlim Benchmark_Log_EndToEnd_Throughput_MRE = null;
    private ILogSink Benchmark_Log_EndToEnd_Throughput_Sink = null;

    [GlobalSetup(Targets = [nameof(Log_EndToEnd_Throughput)])]
    public void Log_EndToEnd_Throughput_Setup()
    {
        Benchmark_Log_EndToEnd_Throughput_Handler = new LogHandler();
        Benchmark_Log_EndToEnd_Throughput_MRE = new();
        Benchmark_Log_EndToEnd_Throughput_Sink = new CounterSink(Benchmark_Log_EndToEnd_Throughput_MRE, 50000);
        Benchmark_Log_EndToEnd_Throughput_Source = new StdLogger()
        {
            Handler = Benchmark_Log_EndToEnd_Throughput_Handler,
            Sinks = [Benchmark_Log_EndToEnd_Throughput_Sink]
        };
    }
    [GlobalCleanup(Targets = [nameof(Log_EndToEnd_Throughput)])]
    public void Log_EndToEnd_Throughput_Cleanup()
    {
        Benchmark_Log_EndToEnd_Throughput_Handler.Dispose();
        Benchmark_Log_EndToEnd_Throughput_MRE.Dispose();
    }

    [Benchmark(Baseline = true)]
    public void Empty() { }

    [Benchmark]
    public void Log_EnqueueOnly()
    {
        Benchmark_Log_EnqueueOnly_Source.Info("Hello world!");
    }

    [Benchmark(OperationsPerInvoke = 50000)]
    public void Log_EndToEnd_Throughput()
    {
        for (int i = 0; i < 50000; i++)
        {
            Benchmark_Log_EndToEnd_Throughput_Source.Info("Hello world!");
        }
        Benchmark_Log_EndToEnd_Throughput_MRE.Wait();
    }
}