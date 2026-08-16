namespace JSL.Logging;

/// <summary>
/// Defines <see cref="LogObject"/> formatting rules
/// </summary>
public interface ILogFormatter
{
    public string Format(LogObject log);
}