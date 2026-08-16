using Microsoft.Extensions.Logging;
using JSL.Logging.Handlers;

namespace JSL.Logging;

/// <summary>
/// Define a new logging source
/// </summary>
public interface ILogSource : ILogger
{
    /// <summary>
    /// A unique name to identify this logger by
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// A short name used to identify this source's logs
    /// </summary>
    public string Alias { get; }
    /// <summary>
    /// The sinks that logs from this source will go to when dumped to a handler (<see cref="ILogHandler.Dump"/>)
    /// </summary>
    public ILogSink[] Sinks { get; set; }
    /// <summary>
    /// Specify a handler that the log source can reference
    /// </summary>
    /// <remarks>An implementation of <see cref="ILogSource"/> is not required to utilize the <see cref="Handler"/> provided</remarks>
    public ILogHandler? Handler { get; set; }
}