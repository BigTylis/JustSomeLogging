// Copyright (c) 2026, BigTylis
// SPDX-License-Identifier: BSD-3-Clause

using JSL.Logging.Formatters;
using System.Diagnostics;

namespace JSL.Logging.Sinks;

/// <summary>
/// A sink that routes logs to the debug console. <see cref="LoggingConfiguration.EnableDebugConsoleSink"/> must be enabled for this to output logs!
/// </summary>
public class DebugConsoleSink : ILogSink
{
    [ThreadStatic] internal static bool SuppressDebugCapture = false;

    protected virtual ILogFormatter formatter { get; set; } = DefaultFormatter.Instance;
    public virtual ILogFormatter? Formatter
    {
        get => formatter;
        set
        {
            if (value is null) formatter = DefaultFormatter.Instance;
            else formatter = value;
        }
    }

    public virtual void Route(ILogObject log)
    {
        if (LoggingConfiguration.EnableDebugConsoleSink)
        {
            string formatted = Formatter!.Format(log);
            SuppressDebugCapture = true;
            Trace.WriteLine(formatted);
            SuppressDebugCapture = false;
        }
    }

    public readonly struct DebugCaptureSuppressionScope : IDisposable
    {
        public DebugCaptureSuppressionScope() => SuppressDebugCapture = true;
        public void Dispose() => SuppressDebugCapture = false;
    }
}