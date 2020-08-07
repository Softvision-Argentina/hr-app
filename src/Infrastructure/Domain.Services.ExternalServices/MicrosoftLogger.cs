// <copyright file="MicrosoftLogger.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.ExternalServices
{
    using System;
    using Core;
    using Microsoft.Extensions.Logging;

    public class MicrosoftLogger<T> : ILog<T>
    {
        private readonly ILogger logger;

        public MicrosoftLogger(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public bool IsDebugEnabled => this.logger.IsEnabled(LogLevel.Debug);

        public bool IsInformationEnabled => this.logger.IsEnabled(LogLevel.Information);

        public bool IsWarningEnabled => this.logger.IsEnabled(LogLevel.Warning);

        public bool IsTraceEnabled => this.logger.IsEnabled(LogLevel.Trace);

        public void LogCritical(string message, params object[] args)
        {
            this.logger.LogCritical(message, args);
        }

        public void LogCritical(Exception exception, string message, params object[] args)
        {
            this.logger.LogCritical(new EventId(), exception, message, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            this.logger.LogDebug(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            this.logger.LogError(message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            this.logger.LogError(new EventId(), exception, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            this.logger.LogInformation(message, args);
        }

        public void LogTrace(string message, params object[] args)
        {
            this.logger.LogTrace(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            this.logger.LogWarning(message, args);
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            this.logger.LogWarning(new EventId(), exception, message, args);
        }
    }
}
