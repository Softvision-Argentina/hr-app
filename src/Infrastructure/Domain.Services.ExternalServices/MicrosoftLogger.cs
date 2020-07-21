using Core;
using Microsoft.Extensions.Logging;
using System;

namespace Domain.Services.ExternalServices
{
    public class MicrosoftLogger<T> : ILog<T>
    {
        private readonly ILogger _logger;

        public MicrosoftLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public bool IsDebugEnabled => _logger.IsEnabled(LogLevel.Debug);

        public bool IsInformationEnabled => _logger.IsEnabled(LogLevel.Information);

        public bool IsWarningEnabled => _logger.IsEnabled(LogLevel.Warning);

        public bool IsTraceEnabled => _logger.IsEnabled(LogLevel.Trace);

        public void LogCritical(string message, params object[] args)
        {
            _logger.LogCritical(message, args);
        }

        public void LogCritical(Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(new EventId(), exception, message, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(new EventId(), exception, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogTrace(string message, params object[] args)
        {
            _logger.LogTrace(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(new EventId(), exception, message, args);
        }
    }
}
