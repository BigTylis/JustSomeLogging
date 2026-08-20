// Copyright (c) 2026, BigTylis
// SPDX-License-Identifier: BSD-3-Clause

using JSL.Logging.Sinks;

namespace JSL;

/// <summary>
/// Configs for the default JSL implementations
/// </summary>
public static class LoggingConfiguration
{
    /// <summary>
    /// Enables <see cref="DebugConsoleSink"/> if true. Default false
    /// </summary>
    public static bool EnableDebugConsoleSink = false;
}
