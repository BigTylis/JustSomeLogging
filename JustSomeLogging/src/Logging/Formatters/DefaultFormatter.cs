using JSL.Extensions;
using System.Text;
using System.Threading;

namespace JSL.Logging.Formatters;

public class DefaultFormatter : ILogFormatter
{
    /// <summary>
    /// Shared singleton instance
    /// </summary>
    public static readonly DefaultFormatter Instance = new();

    protected virtual ThreadLocal<StringBuilder> stringBuilder { get; } = new(() => new StringBuilder());
    public virtual string Format(LogObject log)
    {
        var builder = stringBuilder.Value;
        builder.Clear();

        builder.Append(log.Timestamp.ToString("HH:mm:ss.fff"));
        builder.Append(" [");
        builder.Append(log.LogLevel.ToStringShorten());
        builder.Append("] ");
        builder.Append("[");
        builder.Append(log.Provider.Alias);
        builder.Append("]");

        if(log.ThreadName != null)
        {
            builder.Append(" [");
            builder.Append(log.ThreadName);
            builder.Append("]");
        }

        builder.Append(" - ");
        builder.Append(log.Message);

        if(log.Exception != null)
        {
            builder.Append("\n     Trace: ");
            builder.Append(log.Exception.ToString());
            builder.Append("\n");
        }

        return builder.ToString();
    } 
}