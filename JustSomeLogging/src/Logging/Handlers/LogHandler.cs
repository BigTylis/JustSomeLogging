// Copyright (c) 2026, BigTylis
// SPDX-License-Identifier: BSD-3-Clause

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace JSL.Logging.Handlers;

/// <summary>
/// Default implementation of an <see cref="ILogHandler"/>
/// </summary>
public class LogHandler : ILogHandler, IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Shared singleton instance
    /// </summary>
    public static readonly LogHandler Instance = new LogHandler().HookToProcessExit();
    public virtual Channel<ILogObject> LoggingChannel { get; } = Channel.CreateUnbounded<ILogObject>(new UnboundedChannelOptions()
    {
        SingleReader = true
    });
    protected virtual TaskCompletionSource<bool> flushCompleteEvent { get; } = new(false);

    public LogHandler()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        var taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
        taskFactory.StartNew(async () =>
        {
            await foreach (var log in LoggingChannel.Reader.ReadAllAsync())
            {
                for (int i = 0; i < log.Source.Sinks.Length; i++)
                {
                    var sink = log.Source.Sinks[i];
                    try
                    {
                        sink.Route(log);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"An error occured when trying to route log into a sink ({sink.GetType().FullName}): {ex}");
                    }
                }
            }
            flushCompleteEvent.TrySetResult(true);
        });
    }

    public virtual void Dump(ILogObject log) => LoggingChannel.Writer.TryWrite(log);

    /// <summary>
    /// Chain method to auto dispose/flush this handler on <see cref="AppDomain.ProcessExit"/>
    /// </summary>
    public virtual LogHandler HookToProcessExit()
    {
        AppDomain.CurrentDomain.ProcessExit += (object? sender, EventArgs e) => Dispose();
        return this;
    }

    public virtual void Dispose()
    {
        LoggingChannel.Writer.TryComplete();
        flushCompleteEvent.Task.Wait();
    }
    public virtual async ValueTask DisposeAsync()
    {
        LoggingChannel.Writer.TryComplete();
        await flushCompleteEvent.Task;
    }
}