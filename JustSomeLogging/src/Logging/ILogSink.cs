// Copyright (c) 2026, BigTylis
// SPDX-License-Identifier: BSD-3-Clause

namespace JSL.Logging;

/// <summary>
/// Define a new logging sink
/// </summary>
public interface ILogSink
{
    /// <summary>
    /// Specify a formatter that this log sink can reference
    /// </summary>
    /// <remarks>An implementation of <see cref="ILogSink"/> is not required to utilize the <see cref="ILogFormatter"/> provided</remarks>
    public ILogFormatter? Formatter { get; }
    /// <summary>
    /// Define logic as to where this log is sent.
    /// </summary>
    /// <remarks>Execution context is defined by the utilized <see cref="ILogHandler"/>, so you may need additional synchronization mechanisms to support multi-threaded log handlers</remarks>
    public void Route(ILogObject log);
}