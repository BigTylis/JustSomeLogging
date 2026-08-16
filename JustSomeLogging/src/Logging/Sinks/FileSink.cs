using JSL.Logging.Formatters;
using System.Text;
using System.Threading.Channels;

namespace JSL.Logging.Sinks;

/// <summary>
/// A sink that routes outputs to files as text with its own background thread. Supports interweaving multiple sources into one file.
/// </summary>
public class FileSink : ILogSink, IDisposable, IAsyncDisposable
{
    private static UTF8Encoding DefaultEncoding = new(encoderShouldEmitUTF8Identifier: false);

    protected virtual ILogFormatter formatter { get; set; } = DefaultFormatter.Instance;
    public virtual ILogFormatter Formatter
    {
        get => formatter;
        set
        {
            if (value is null) formatter = DefaultFormatter.Instance;
            else formatter = value;
        }
    }

    /// <summary>
    /// Should flush straight to disk. Default true
    /// </summary>
    virtual public bool FlushToDisk { get; init; } = true;
    /// <summary>
    /// Mappings define where logs from different <see cref="ILogSource"/>s are written to
    /// </summary>
    virtual required public Source2FileMapping[] FileMappings { get; init; } = [];
    /// <summary>
    /// There must be N logs buffered before flushing to all writers
    /// </summary>
    /// <remarks><![CDATA[<= 1 will flush every time]]></remarks>
    virtual required public int BufferedCountBeforeFlush { get; init; } = 50;

    protected virtual TaskCompletionSource<bool> flushCompleteEvent { get; } = new(false);
    protected virtual Channel<LogObject> fileChannel { get; } = Channel.CreateUnbounded<LogObject>(new UnboundedChannelOptions
    {
        SingleReader = true
    });
    protected virtual Dictionary<string, FileStream> pathToStreamMappings { get; } = [];
    protected virtual Dictionary<string, List<StreamWriter>> sourceToWriterMappings { get; } = [];
    protected virtual List<StreamWriter> allWriters { get; } = [];
    protected virtual List<LogObject> receivedBufferedLogs { get; } = [];

    public FileSink() => Setup();

    protected virtual void Setup()
    {
        var taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
        taskFactory.StartNew(async () =>
        {
            foreach (var mapping in FileMappings)
            {
                if (!pathToStreamMappings.ContainsKey(mapping.FileName))
                    pathToStreamMappings.Add(mapping.FileName, new FileStream(mapping.FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));

                if (!sourceToWriterMappings.ContainsKey(mapping.ProviderName))
                    sourceToWriterMappings.Add(mapping.ProviderName, []);

                var writersCollection = sourceToWriterMappings[mapping.ProviderName];
                var writer = new StreamWriter(pathToStreamMappings[mapping.FileName], mapping.Encoding ?? DefaultEncoding);
                writersCollection.Add(writer);

                allWriters.Add(writer);
            }

            await foreach(var log in fileChannel.Reader.ReadAllAsync())
            {
                if(sourceToWriterMappings.ContainsKey(log.Provider.Name)) receivedBufferedLogs.Add(log);
                if (receivedBufferedLogs.Count >= BufferedCountBeforeFlush) emptyBuffer();
            }

            emptyBuffer();
            flushCompleteEvent.TrySetResult(true);
        });
    }

    public virtual void Route(LogObject log) => fileChannel.Writer.TryWrite(log);

    /// <summary>
    /// Chain method to auto dispose/flush this sink on <see cref="AppDomain.ProcessExit"/>
    /// </summary>
    public virtual FileSink HookToProcessExit()
    {
        AppDomain.CurrentDomain.ProcessExit += (s, e) => Dispose();
        return this;
    }

    protected virtual void emptyBuffer()
    {
        for (int i = 0; i < receivedBufferedLogs.Count; i++)
        {
            var bufferedLog = receivedBufferedLogs[i];
            string formatted = Formatter.Format(bufferedLog);

            var writers = sourceToWriterMappings[bufferedLog.Provider.Name];
            for (int ii = 0; ii < writers.Count; ii++)
            {
                var writer = writers[ii];
                writer.WriteLine(formatted);
            }
        }
        receivedBufferedLogs.Clear();

        for (int i = 0; i < allWriters.Count; i++)
        {
            var writer = allWriters[i];
            writer.Flush();
        }
        foreach (var stream in pathToStreamMappings.Values)
        {
            stream.Flush(flushToDisk: FlushToDisk);
        }
    }

    public virtual void Dispose()
    {
        fileChannel.Writer.TryComplete();
        flushCompleteEvent.Task.Wait();
    }
    public virtual async ValueTask DisposeAsync()
    {
        fileChannel.Writer.TryComplete();
        await flushCompleteEvent.Task;
    }

    public readonly struct Source2FileMapping
    {
        /// <summary>
        /// Name of the provider whos logs will be sent to the file specified
        /// </summary>
        required public string ProviderName { get; init; }
        required public string FileName { get; init; }
        /// <summary>
        /// Default UTF8
        /// </summary>
        public Encoding? Encoding { get; init; }
    }
}