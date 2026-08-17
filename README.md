# Overview

JustSomeLogging (JSL) is an incredibly simple logging library for C#. Its built as a framework for you to implement logging your own way, without worrying about the abstract structural details. JSL is designed to revolve around the following 
pattern: Source -> Handler -> Sink. Dont want to design your own? You can use the pre-built ready to go implementations! If you do want to design your own, the pre-built types provide an excellent reference for how you should structure your design.

## Why JSL?

_Do you want..._
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

Compatible with [MessagePack](https://github.com/MessagePack-CSharp/MessagePack-CSharp) via the DataContract attribute.




# Quick Start

Working with LogHandlers
```c#
// Obtain a default LogHandler
var handler = new LogHandler();
// or
var handler = LogHandler.Instance;
```

Configuring the default LogHandler
```c#
// LogHandler has one configuration
var handler = new LogHandler().HookToProcessExit(); // <-- HookToProcessExit adds an event hook to AppDomain.Current.ProcessExit, which will call Dispose() on the handler

// By default, this singleton version always uses HookToProcessExit()
var handler = LogHandler.Instance;

// Why Dispose like this? Disposing the LogHandler here will halt the exit until all logs are flushed to their sinks
```
