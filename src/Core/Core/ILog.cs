using System;

namespace Core
{
    public interface ILog<T>
    {
        bool IsDebugEnabled { get; }
        bool IsInformationEnabled { get; }
        bool IsWarningEnabled { get; }
        bool IsTraceEnabled { get; }

        void LogDebug(string message, params object[] args);
        void LogInformation(string message, params object[] args);
        void LogTrace(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogWarning(Exception exception, string message, params object[] args);
        void LogCritical(string message, params object[] args);
        void LogCritical(Exception exception,string message, params object[] args);
        void LogError(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
    }
}
