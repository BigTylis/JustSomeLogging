using JSL.Logging.Loggers;
using JSL.Logging.Sinks;
using JSL.Extensions;
using System.Diagnostics;
using JSL.Logging.Handlers;

Thread.CurrentThread.Name = "MainThread";

// handlers
var handler = new LogHandler().HookToProcessExit();

// sinks
var consoleSink = new DebugConsoleSink();
string processDir = Path.GetDirectoryName(Environment.ProcessPath)!;
var fileSink = new FileSink()
{
    BufferedCountBeforeFlush = 100,
    FlushToDisk = true,
    FileMappings = [new FileSink.Source2FileMapping()
    {
        SourceNames = [nameof(StdLogger), nameof(DebugCapturer)],
        FileNames = [Path.Combine(processDir, "mylog.txt"), Path.Combine(processDir, "mylogcopy.txt")]
    },
    new FileSink.Source2FileMapping()
    {
        SourceNames = [nameof(DebugCapturer)],
        FileNames = [Path.Combine(processDir, "debugcaptureonly.txt")],
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