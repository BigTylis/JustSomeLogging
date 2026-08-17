// Copyright (c) 2026, BigTylis
// SPDX-License-Identifier: BSD-3-Clause

using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace JSL.Logging;

/// <summary>
/// Represents a single log message
/// </summary>
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