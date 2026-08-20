<p align="center">
    <img src="icon-v2.png" alt="icon"/>
</p>

# Overview

JustSomeLogging (JSL) is an incredibly simple logging library for C#. Its built as a framework for you to implement logging your own way, without worrying about the abstract structural details. JSL is designed to revolve around the following 
pattern: Source -> Handler -> Sink. Dont want to design your own? You can use the pre-built ready to go implementations! If you do want to design your own, the pre-built types provide an excellent reference for how you should structure your design.

TLDR just let me install: [go to install](https://github.com/BigTylis/JustSomeLogging/tree/main#installation)

## Why JSL?

* Dead-simple configuration
* Entire control flow in your hands
* Small learning curve
* Awesome performance

## Performance

With your own implementations, you completely control the performance!
But what about the default implementations? 

<img width="634" height="95" alt="image" src="https://github.com/user-attachments/assets/d252f270-3375-4147-97b5-e6f6dec595a5" />

<sub>**_Log_EnqueueOnly_** - measures the actual caller thread time taken to send off the log</sub>

<sub>**_Log_EndToEnd_Throughput_** - measures the average full cycle completion time, Source -> Handler -> Sink (no formatting in sink), in a high throughput concurrent processing situation (50k logs)</sub>

## Compatibility

Built around
```c#
Microsoft.Extensions.Logging.ILogger
```
so it should be generally compatibly with most other logging systems.

Serialization compatability with [MessagePack](https://github.com/MessagePack-CSharp/MessagePack-CSharp) via the DataContract attribute.




# Quick Start

Getting the default LogHandler
```c#
var handler = new LogHandler();
// or
var handler = LogHandler.Instance;
```

Configuring the default LogHandler
```c#
// LogHandler has one configuration
var handler = new LogHandler().HookToProcessExit();

// HookToProcessExit adds an event hook to AppDomain.Current.ProcessExit,
// which will call Dispose() on the handler

// By default, this singleton version always uses HookToProcessExit()
var handler = LogHandler.Instance;

// Why Dispose like this? Disposing the LogHandler here will halt the exit until all logs are flushed to their sinks
```

Create a log source
```c#
var source = new StdLogger()
{
    Handler = handler, // Specify our handler here, OR leave null to use the singleton default LogHandler
};
```

Now add some sinks
```c#
#if DEBUG
        // Required for DebugConsoleSink to work.
        // This configuration is off by default!
        LoggingConfiguration.EnableDebugConsoleSink = true;
#else
        LoggingConfiguration.EnableDebugConsoleSink = false;
#endif

// A sink that routes to console
var consoleSink = new DebugConsoleSink()
{
    Formatter = DefaultFormatter.Instance
};

// A sink that routes to various file destinations per source
var fileSink = new FileSink()
{
    BufferedCountBeforeFlush = 100, // How many logs must be buffered before flushing the underlying streams
    FlushToDisk = true,

    FileMappings = [new FileSink.Source2FileMapping
    {
        FileName = "C:\\MyFile.txt",
        SourceName = nameof(StdLogger), // You want the ILogSource.Name here, which in this case is the type name
        Encoding = System.Text.Encoding.Unicode // Defaults to UTF8 if not specified
    }],

    Formatter = null // No formatter means singleton instance of DefaultFormatter
}.HookToProcessExit(); // File sink uses its own thread, and therefore can also be hooked to exit to flush

// Tip: You can interweave multiple source's logs into a single file if you want
// but for this example I just do one source one file
```

Add the desired sinks to your source instance
```c#
source.Sinks = [consoleSink, fileSink];
```

And run!
```c#
// This log will be sent to both the debug console, and to the file buffering thread to be saved
source.Info("Hello world!");
```

# Details

## What actually can a log object store?

Since logs in JSL are not just strings, additional context can be captured, formatted, and serialized.

LogObject uses the following implementation:
```c#
[DataContract]
public readonly struct LogObject : ILogObject
{
    [DataMember(Name = "src")] required public ILogSource Source { get; init; }
    [DataMember(Name = "msg")] required public string Message { get; init; }
    [DataMember(Name = "time")] required public DateTime Timestamp { get; init; }
    [DataMember(Name = "ll")] required public LogLevel LogLevel { get; init; }
    [DataMember(Name = "thr")] public string? ThreadName { get; init; }
    [DataMember(Name = "ex")] public Exception? Exception { get; init; }
    public StackFrame? StackFrame { get; }
    [DataMember(Name = "stkfs")] public string? StackFrameString { get; }

    public LogObject(StackFrame? stackFrame = null)
    {
        StackFrame = stackFrame;
        StackFrameString = stackFrame?.ToString();
    }
}
```

_You can make your own ILogObject to expand upon context storage if this is not enough._

## Support

JSL is built on Netstandard 2.0, so it can work [just about anywhere](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0#tabpanel_1_net-standard-2-0). JSL doesn't have any platform specific restrictions.

## Installation 

### Get it on NuGet:

Use the command:

```ps1
dotnet package add JustSomeLogging
```

[![NuGet Version](https://img.shields.io/nuget/v/JustSomeLogging)](https://www.nuget.org/packages/JustSomeLogging/)

### Or from the releases:

Add a reference to the DLL

[![GitHub Release](https://img.shields.io/github/v/release/BigTylis/JustSomeLogging)](https://github.com/BigTylis/JustSomeLogging/releases)

---

[![GitHub All Releases](https://img.shields.io/github/downloads/BigTylis/JustSomeLogging/total.svg?label="Github%20Downloads")](https://github.com/BigTylis/JustSomeLogging/releases)
![NuGet Downloads](https://img.shields.io/nuget/dt/JustSomeLogging?label="NuGet%20Downloads"&color="blue")
![GitHub Repo stars](https://img.shields.io/github/stars/BigTylis/JustSomeLogging)

