// Copyright (c) 2026, BigTylis
// SPDX-License-Identifier: BSD-3-Clause

using Microsoft.Extensions.Logging;

namespace JSL.Logging;

public interface ILogObject
{
    public ILogSource Source { get; }
    public string Message { get; }
    public DateTime Timestamp { get; }
    public LogLevel LogLevel { get; }
    public string? ThreadName { get; }
    public Exception? Exception { get; }
}