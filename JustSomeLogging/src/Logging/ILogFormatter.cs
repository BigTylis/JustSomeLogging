// Copyright (c) 2026, BigTylis
// SPDX-License-Identifier: BSD-3-Clause

namespace JSL.Logging;

/// <summary>
/// Defines <see cref="ILogObject"/> formatting rules
/// </summary>
public interface ILogFormatter
{
    public string Format(ILogObject log);
}