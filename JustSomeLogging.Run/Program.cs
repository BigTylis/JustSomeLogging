using JSL.Logging.Loggers;
using JSL.Logging.Sinks;
using JSL.Extensions;
using System.Diagnostics;
using JSL.Logging.Handlers;
using JSL.Logging.Formatters;

Thread.CurrentThread.Name = "MainThread";

// handlers
var handler = new LogHandler().HookToProcessExit();

// sinks
var consoleSink = new DebugConsoleSink();
var fileSink = new FileSink()
{
    BufferedCountBeforeFlush = 100,
    FlushToDisk = true,
    FileMappings = [new FileSink.Source2FileMapping()
    {
        FileName = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "mylog.txt"),
        SourceName = nameof(StdLogger)
    },
    new FileSink.Source2FileMapping()
    {
        FileName = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "mylog.txt"),
        SourceName = nameof(DebugCapturer)
    },
    new FileSink.Source2FileMapping()
    {
        FileName = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "debugcaptureonly.txt"),
        SourceName = nameof(DebugCapturer),
    }],
}.HookToProcessExit();

// sources
var capturer = new DebugCapturer()
{
    Sinks = [consoleSink, fileSink],
    Handler = handler
};
Trace.Listeners.Add(capturer);
var std = new StdLogger()
{
    Sinks = [consoleSink, fileSink],
    Handler = handler
};

// test
Debug.WriteLine("capture me");
for (int i = 0; i < 1000; i++)
{
    std.Warn($"log #{i}");
}

using(new DebugConsoleSink.DebugCaptureSuppressionScope())
{
    Debug.WriteLine("finished calling warn");
}