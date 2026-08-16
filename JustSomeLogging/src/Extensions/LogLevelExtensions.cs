using JSL.Logging;
using Microsoft.Extensions.Logging;

namespace JSL.Extensions;

public static class LogLevelExtensions
{
    public static string ToStringShorten(this LogLevel self) => self switch
    {
        LogLevel.Error => "Error",
        LogLevel.Warning => "Warn",
        LogLevel.Information => "Info",
        LogLevel.Trace => "Verbose",
        LogLevel.Debug => "Debug",
        LogLevel.Critical => "Fatal",
        LogLevel.None => "Unspecified"
    };

    public static LogLevelFlags ToFlag(this LogLevel self) => self switch
    {
        LogLevel.Error => LogLevelFlags.Error,
        LogLevel.Warning => LogLevelFlags.Warning,
        LogLevel.Information => LogLevelFlags.Information,
        LogLevel.Trace => LogLevelFlags.Trace,
        LogLevel.Debug => LogLevelFlags.Debug,
        LogLevel.Critical => LogLevelFlags.Critical,
        LogLevel.None => LogLevelFlags.None
    };
}