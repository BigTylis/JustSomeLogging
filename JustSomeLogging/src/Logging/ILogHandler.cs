using System.Threading.Channels;

namespace JSL.Logging;

public interface ILogHandler
{
    /// <summary>
    /// Define initialization for this handler, such as starting background threads to read from the <see cref="LoggingChannel"/>, or hooking into app shutdown events for immediate flushing
    /// </summary>
    public void Initialize();
    /// <summary>
    /// Define how this handler manages logs it recieves.
    /// </summary>
    public void Dump(LogObject log);
}