// Copyright (c) 2026, BigTylis
// SPDX-License-Identifier: BSD-3-Clause

using JSL.Logging;
using Microsoft.Extensions.Logging;
using System;

namespace JSL.Extensions;

public static class ILogSourceExtensions
{
    /// <inheritdoc cref="LoggerExtensions.LogInformation"/>
    public static void Info(this ILogSource self, string? message, params object?[] args) => self.LogInformation(message, args);

    /// <inheritdoc cref="LoggerExtensions.LogInformation"/>
    public static void Info(this ILogSource self, EventId eventId, string? message, params object?[] args) => self.LogInformation(eventId, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogInformation"/>
    public static void Info(this ILogSource self, Exception? exception, string? message, params object?[] args) => self.LogInformation(exception, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogInformation"/>
    public static void Info(this ILogSource self, EventId eventId, Exception? exception, string? message, params object?[] args) => self.LogInformation(eventId, exception, message, args);

    /// <inheritdoc cref="LoggerExtensions.LogDebug"/>
    public static void Debug(this ILogSource self, string? message, params object?[] args) => self.LogDebug(message, args);
    /// <inheritdoc cref="LoggerExtensions.LogDebug"/>
    public static void Debug(this ILogSource self, EventId eventId, string? message, params object?[] args) => self.LogDebug(eventId, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogDebug"/>
    public static void Debug(this ILogSource self, Exception? exception, string? message, params object?[] args) => self.LogDebug(exception, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogDebug"/>
    public static void Debug(this ILogSource self, EventId eventId, Exception? exception, string? message, params object?[] args) => self.LogDebug(eventId, exception, message, args);

    /// <inheritdoc cref="LoggerExtensions.LogTrace"/>
    public static void Trace(this ILogSource self, string? message, params object?[] args) => self.LogTrace(message, args);
    /// <inheritdoc cref="LoggerExtensions.LogTrace"/>
    public static void Trace(this ILogSource self, EventId eventId, string? message, params object?[] args) => self.LogTrace(eventId, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogTrace"/>
    public static void Trace(this ILogSource self, Exception? exception, string? message, params object?[] args) => self.LogTrace(exception, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogTrace"/>
    public static void Trace(this ILogSource self, EventId eventId, Exception? exception, string? message, params object?[] args) => self.LogTrace(eventId, exception, message, args);

    /// <inheritdoc cref="LoggerExtensions.LogWarning"/>
    public static void Warn(this ILogSource self, string? message, params object?[] args) => self.LogWarning(message, args);
    /// <inheritdoc cref="LoggerExtensions.LogWarning"/>
    public static void Warn(this ILogSource self, EventId eventId, string? message, params object?[] args) => self.LogWarning(eventId, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogWarning"/>
    public static void Warn(this ILogSource self, Exception? exception, string? message, params object?[] args) => self.LogWarning(exception, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogWarning"/>
    public static void Warn(this ILogSource self, EventId eventId, Exception? exception, string? message, params object?[] args) => self.LogWarning(eventId, exception, message, args);

    /// <inheritdoc cref="LoggerExtensions.LogError"/>
    public static void Error(this ILogSource self, string? message, params object?[] args) => self.LogError(message, args);
    /// <inheritdoc cref="LoggerExtensions.LogError"/>
    public static void Error(this ILogSource self, EventId eventId, string? message, params object?[] args) => self.LogError(eventId, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogError"/>
    public static void Error(this ILogSource self, Exception? exception, string? message, params object?[] args) => self.LogError(exception, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogError"/>
    public static void Error(this ILogSource self, EventId eventId, Exception? exception, string? message, params object?[] args) => self.LogError(eventId, exception, message, args);

    /// <inheritdoc cref="LoggerExtensions.LogCritical"/>
    public static void Fatal(this ILogSource self, string? message, params object?[] args) => self.LogCritical(message, args);
    /// <inheritdoc cref="LoggerExtensions.LogCritical"/>
    public static void Fatal(this ILogSource self, EventId eventId, string? message, params object?[] args) => self.LogCritical(eventId, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogCritical"/>
    public static void Fatal(this ILogSource self, Exception? exception, string? message, params object?[] args) => self.LogCritical(exception, message, args);
    /// <inheritdoc cref="LoggerExtensions.LogCritical"/>
    public static void Fatal(this ILogSource self, EventId eventId, Exception? exception, string? message, params object?[] args) => self.LogCritical(eventId, exception, message, args);
}