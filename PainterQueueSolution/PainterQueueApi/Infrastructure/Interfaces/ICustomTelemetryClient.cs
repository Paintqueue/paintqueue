namespace PainterQueueApi.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines a contract for a telemetry client that provides methods for logging exceptions, warnings, and
    /// informational messages.
    /// </summary>
    /// <remarks>This interface supports logging with varying levels of detail, including optional payloads
    /// and additional data. Implementations of this interface are expected to handle the logging of telemetry data in a
    /// consistent and thread-safe manner.</remarks>
    /// <typeparam name="T">The type of the payload that can be included with log entries to provide additional context.</typeparam>
    public interface ICustomTelemetryClient<T>
    {
        /// <summary>
        /// Logs the specified exception along with an additional message for context.
        /// </summary>
        /// <remarks>This method is typically used to record exceptions in a logging system for diagnostic
        /// purposes.  Ensure that <paramref name="ex"/> is not <see langword="null"/> to avoid runtime
        /// errors.</remarks>
        /// <param name="ex">The exception to be logged. Cannot be <see langword="null"/>.</param>
        /// <param name="message">A custom message providing additional context for the exception. Can be <see langword="null"/> or empty.</param>
        void LogException(Exception ex, string message);

        /// <summary>
        /// Logs an exception along with a payload and a custom message.
        /// </summary>
        /// <remarks>This method is typically used to log exceptions with additional context for debugging
        /// or error tracking purposes. Ensure that the <paramref name="ex"/> and <paramref name="message"/> parameters
        /// are not <see langword="null"/> to avoid runtime errors.</remarks>
        /// <param name="ex">The exception to be logged. Cannot be <see langword="null"/>.</param>
        /// <param name="payload">The additional data or context associated with the exception. Can be <see langword="null"/> if no payload is
        /// provided.</param>
        /// <param name="message">A custom message to include in the log entry. Cannot be <see langword="null"/> or empty.</param>
        void LogException(Exception ex, T payload, string message);

        /// <summary>
        /// Logs the specified exception along with additional contextual data and a custom message.
        /// </summary>
        /// <remarks>This method is typically used to log exceptions with supplementary information  to
        /// aid in debugging or error tracking. Ensure that the <paramref name="ex"/> parameter  is not <see
        /// langword="null"/> to avoid runtime errors.</remarks>
        /// <param name="ex">The exception to be logged. Cannot be <see langword="null"/>.</param>
        /// <param name="additionalData">A dictionary containing additional contextual information to include in the log.  Keys represent the context
        /// names, and values represent the associated data.  Can be <see langword="null"/> if no additional data is
        /// provided.</param>
        /// <param name="message">A custom message to include in the log entry. Can be <see langword="null"/> or empty.</param>
        void LogException(Exception ex, Dictionary<string, object> additionalData, string message);

        /// <summary>
        /// Logs an exception along with a payload, additional contextual data, and a custom message.
        /// </summary>
        /// <remarks>This method is intended to capture detailed information about an exception, including
        /// optional contextual data, to assist in debugging and error analysis. Ensure that sensitive information is
        /// not included in the payload or additional data.</remarks>
        /// <param name="ex">The exception to be logged. Cannot be <see langword="null"/>.</param>
        /// <param name="payload">The payload associated with the exception, providing additional context. Can be <see langword="null"/> if no
        /// payload is available.</param>
        /// <param name="additionalData">A dictionary containing key-value pairs of additional contextual information. Can be <see langword="null"/>
        /// if no additional data is provided.</param>
        /// <param name="message">A custom message to include in the log entry. Can be <see langword="null"/> or empty if no message is
        /// provided.</param>
        void LogException(Exception ex, T payload, Dictionary<string, object> additionalData, string message);

        /// <summary>
        /// Logs a warning message to the configured logging system.
        /// </summary>
        /// <param name="WarningMessage">The warning message to be logged. Cannot be null or empty.</param>
        void LogWarning(string WarningMessage);

        /// <summary>
        /// Logs a warning message along with an associated payload.
        /// </summary>
        /// <param name="WarningMessage">The warning message to log. Cannot be null or empty.</param>
        /// <param name="payload">The payload containing additional information related to the warning.</param>
        void LogWarning(string WarningMessage, T payload);

        /// <summary>
        /// Logs a warning message along with optional additional data for context.
        /// </summary>
        /// <remarks>Use this method to log warnings that require additional context for debugging or
        /// analysis. If <paramref name="additionalData"/> is provided, ensure that the keys are unique and the values
        /// are serializable if the logging system persists the data.</remarks>
        /// <param name="WarningMessage">The warning message to log. This value cannot be null or empty.</param>
        /// <param name="additionalData">A dictionary containing additional contextual information to include with the log entry. Keys represent the
        /// names of the data fields, and values represent their corresponding data. This parameter can be null if no
        /// additional data is provided.</param>
        void LogWarning(string WarningMessage, Dictionary<string, object> additionalData);

        /// <summary>
        /// Logs a warning message along with an associated payload and additional contextual data.
        /// </summary>
        /// <param name="WarningMessage">The warning message to log. Cannot be null or empty.</param>
        /// <param name="payload">The payload object associated with the warning. This can provide additional context for the warning.</param>
        /// <param name="additionalData">A dictionary containing additional key-value pairs that provide context for the warning.  Keys must be
        /// unique, and values can be any object. This parameter can be null if no additional data is provided.</param>
        void LogWarning(string WarningMessage, T payload, Dictionary<string, object> additionalData);

        /// <summary>
        /// Logs an informational message to the application's logging system.
        /// </summary>
        /// <remarks>This method is typically used to log non-critical information that can help in
        /// understanding the application's flow or diagnosing issues. Ensure that the <paramref name="message"/> is not
        /// null or empty to maintain meaningful log entries.</remarks>
        /// <param name="message">The message to log. This should provide relevant information about the application's state or behavior.</param>
        void LogInformation(string message);

        /// <summary>
        /// Logs an informational message along with an associated payload.
        /// </summary>
        /// <param name="message">The informational message to log. Cannot be null or empty.</param>
        /// <param name="payload">The payload associated with the message. Provides additional context or data.</param>
        void LogInformation(string message, T payload);

        /// <summary>
        /// Logs an informational message along with optional additional data.
        /// </summary>
        /// <param name="message">The informational message to log. Cannot be null or empty.</param>
        /// <param name="additionalData">A dictionary containing additional key-value pairs to include with the log entry.  Can be null if no
        /// additional data is provided.</param>
        void LogInformation(string message, Dictionary<string, object> additionalData);

        /// <summary>
        /// Logs an informational message along with a payload and additional contextual data.
        /// </summary>
        /// <remarks>This method is typically used to log non-critical information that may assist in
        /// debugging or monitoring application behavior. Ensure that the <paramref name="message"/> and <paramref
        /// name="payload"/> provide meaningful and relevant information.</remarks>
        /// <param name="message">The informational message to log. Cannot be null or empty.</param>
        /// <param name="payload">The payload object containing additional information to include in the log entry.</param>
        /// <param name="additionalData">A dictionary of key-value pairs providing additional contextual data for the log entry.  Keys must be unique
        /// and cannot be null. Values can be null if no specific value is required.</param>
        void LogInformation(string message, T payload, Dictionary<string, object> additionalData);
    }
}
