using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Abstractions;
using Newtonsoft.Json;
using PainterQueueApi.Infrastructure.Interfaces;
using System.Text;

namespace PainterQueueApi.Infrastructure.CustomServices
{
    /// <summary>
    /// Provides a custom telemetry client for logging exceptions, warnings, and informational messages with optional
    /// payloads and additional data. This class integrates with Application Insights and a logging framework to track
    /// and log telemetry data.
    /// </summary>
    /// <remarks>This class is designed to simplify the process of logging telemetry data and exceptions while
    /// providing flexibility to include custom payloads and additional metadata. It uses <TelemetryClient> for
    /// Application Insights integration and <ILogger> for structured logging.</remarks>
    /// <typeparam name="T">The type of the payload that can be included with log entries. This allows for attaching additional contextual
    /// information to telemetry events.</typeparam>
    public class CustomTelemetryClient<T> : ICustomTelemetryClient<T>
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger<CustomTelemetryClient<T>> _logger;
        public CustomTelemetryClient(TelemetryClient telemetryClient, ILogger<CustomTelemetryClient<T>> logger)
        {
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Logs the specified exception along with an optional message.
        /// </summary>
        /// <remarks>This method logs the provided exception and message for diagnostic purposes.</remarks>
        /// <param name="ex">The exception to log. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="message">An optional message providing additional context about the exception. Can be <see langword="null"/> or
        /// empty.</param>
        public void LogException(Exception ex, string message)
        {
            LogException(ex, default, null, message);
        }

        /// <summary>
        /// Logs an exception along with an associated payload and a custom message.
        /// </summary>
        /// <remarks>This method logs the provided exception and any additional context, such as a payload
        /// or message, to the configured logging system.</remarks>
        /// <param name="ex">The exception to be logged. Cannot be <see langword="null"/>.</param>
        /// <param name="payload">The payload associated with the exception. Can be <see langword="null"/> if no payload is available.</param>
        /// <param name="message">A custom message providing additional context for the exception. Can be <see langword="null"/> or empty.</param>
        public void LogException(Exception ex, T payload,string message)
        {
            LogException(ex, payload, null, message);
        }

        /// <summary>
        /// Logs the specified exception along with additional contextual data and a custom message.
        /// </summary>
        /// <param name="ex">The exception to be logged. Cannot be <see langword="null"/>.</param>
        /// <param name="additionalData">A dictionary containing additional contextual information to include in the log.  Keys represent the data
        /// labels, and values represent the associated data. Can be <see langword="null"/>.</param>
        /// <param name="message">A custom message to include in the log. Can be <see langword="null"/> or empty.</param>
        public void LogException(Exception ex, Dictionary<string, object> additionalData, string message)
        {
            LogException(ex, default, additionalData, message);
        }

        /// <summary>
        /// Logs an exception along with optional contextual data and a custom message.
        /// </summary>
        /// <remarks>This method logs the exception using the configured logger and tracks it as a
        /// telemetry event.  The exception details, including its type, message, stack trace, and any additional data,
        /// are included in the log.</remarks>
        /// <param name="ex">The exception to log. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="payload">An optional payload object containing additional context about the operation that caused the exception.  If
        /// provided, it will be serialized and included in the log.</param>
        /// <param name="additionalData">An optional dictionary of key-value pairs containing additional data to include in the log.  If <see
        /// langword="null"/>, an empty dictionary will be used.</param>
        /// <param name="message">An optional custom message to include in the log. If not provided, a default message will be used.</param>
        public void LogException(Exception ex, T? payload, Dictionary<string, object>? additionalData = null, string message = "")
        {
            ArgumentNullException.ThrowIfNull(ex);

            // Set properties for tracking event telemetry
            additionalData ??= new Dictionary<string, object>();

            additionalData.Add("Type", ex.GetType().ToString());
            additionalData.Add("Message", ex.Message);
            additionalData.Add("Custom Message", message ?? "No custom message.");
            additionalData.Add("StackTrace", ex.StackTrace ?? "No stack trace.");

            var properties = new Dictionary<string, string>
            {
                { "Type", ex.GetType().ToString() },
                { "Message", ex.Message },
                { "Custom Message", message ?? "No custom message." },
                { "StackTrace", ex.StackTrace ?? "No stack trace." }
            };

            // Add payload to properties if not null
            if (payload != null)
            {
                properties.Add("InputPayload", JsonConvert.SerializeObject(payload));
            }

            // Add properties to string for log exception
            var propertiesSb = new StringBuilder();
            propertiesSb.AppendLine(" Error Properties:");
            
            foreach (var item in properties)
            {
                propertiesSb.AppendLine($" {item.Key}: {item.Value};");
            }

            if (additionalData != null)
            {
                foreach (var item in additionalData)
                {
                    // Add to addtiona data string for log warning
                    propertiesSb.AppendLine($" {item.Key}: {item.Value};");

                    // Add properties for tracking event telemetry
                    properties.Add(item.Key, item.Value?.ToString() ?? "");
                }
            }
            propertiesSb.AppendLine(" End of error properties.");

            _logger.LogError(ex, message: ex.Message + propertiesSb.ToString());
            _telemetryClient.TrackException(ex, properties);
        }

        /// <summary>
        /// Logs a warning message to the configured logging system.
        /// </summary>
        /// <remarks>This method logs a warning message with default settings. To include additional
        /// context or  metadata, use an overload of this method that accepts more parameters.</remarks>
        /// <param name="WarningMessage">The warning message to log. This value cannot be null or empty.</param>
        public void LogWarning(string WarningMessage)
        {
            LogWarning(WarningMessage, default, null);
        }

        /// <summary>
        /// Logs a warning message with optional additional data.
        /// </summary>
        /// <param name="WarningMessage">The warning message to log. Cannot be null or empty.</param>
        /// <param name="additionalData">A dictionary containing additional contextual data to include with the log entry.  Can be null if no
        /// additional data is provided.</param>
        public void LogWarning(string WarningMessage, Dictionary<string, object> additionalData)
        {
            LogWarning(WarningMessage, default, additionalData);
        }

        /// <summary>
        /// Logs a warning message along with an optional payload.
        /// </summary>
        /// <remarks>This method logs a warning message and optionally associates it with a payload.  Use
        /// this method to capture non-critical issues that require attention but do not interrupt the application's
        /// flow.</remarks>
        /// <param name="WarningMessage">The warning message to log. Cannot be null or empty.</param>
        /// <param name="payload">An optional payload of type <typeparamref name="T"/> to include with the warning. Can be null.</param>
        public void LogWarning(string WarningMessage, T payload)
        {
            LogWarning(WarningMessage, payload, null);
        }

        /// <summary>
        /// Logs a warning message along with optional payload and additional data for context.
        /// </summary>
        /// <remarks>This method logs the warning message using the configured logger and tracks the event
        /// in telemetry with the provided properties. The <paramref name="payload"/> and  <paramref
        /// name="additionalData"/> parameters are optional but can be used to enrich  the log entry with more
        /// context.</remarks>
        /// <param name="WarningMessage">The warning message to log. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="payload">An optional payload object containing additional information related to the warning.  If provided, it will
        /// be serialized and included in the log.</param>
        /// <param name="additionalData">An optional dictionary of key-value pairs providing additional contextual data for the warning.  If
        /// provided, the data will be included in the log and tracked in telemetry.</param>
        public void LogWarning(string WarningMessage, T? payload, Dictionary<string, object>? additionalData = null)
        {
            ArgumentNullException.ThrowIfNull(WarningMessage);

            var properties = new Dictionary<string, string>
            {
                { "WarningMessage", WarningMessage }
            };

            if (payload != null)
            {
                properties.Add("InputPayload", JsonConvert.SerializeObject(payload));
            }

            var additionalDataSb = new StringBuilder();
            if (additionalData != null)
            {
                additionalDataSb.AppendLine("  Additional data:");

                foreach (var item in additionalData)
                {
                    // Add to addtiona data string for log warning
                    additionalDataSb.AppendLine($" {item.Key}: {item.Value};");

                    // Add properties for tracking event telemetry
                    properties.Add(item.Key, item.Value?.ToString() ?? "");
                }

                additionalDataSb.AppendLine(" End of additional data.");
            }

            _logger.LogWarning(WarningMessage + additionalDataSb.ToString());

            _telemetryClient.TrackTrace("Warning", properties);
        }

        /// <summary>
        /// Logs an informational message to the configured logging system.
        /// </summary>
        /// <remarks>This method provides a simplified way to log informational messages.  For more
        /// advanced logging scenarios, consider using overloads or additional parameters.</remarks>
        /// <param name="message">The informational message to log. Cannot be null or empty.</param>
        public void LogInformation(string message)
        {
            LogInformation(message, default!, null!);
        }

        /// <summary>
        /// Logs an informational message with optional additional data.
        /// </summary>
        /// <param name="message">The informational message to log. Cannot be null or empty.</param>
        /// <param name="additionalData">A dictionary containing additional contextual data to include with the log entry.  Keys represent data
        /// labels, and values represent the associated data. Can be null.</param>
        public void LogInformation(string message, Dictionary<string, object> additionalData)
        {
            LogInformation(message, default!, additionalData);
        }

        /// <summary>
        /// Logs an informational message along with an associated payload.
        /// </summary>
        /// <param name="message">The informational message to log. Cannot be null or empty.</param>
        /// <param name="payload">The payload object associated with the message. Can be null.</param>
        public void LogInformation(string message, T payload)
        {
            LogInformation(message, payload, null!);
        }

        /// <summary>
        /// Logs an informational message along with an optional payload and additional data.
        /// </summary>
        /// <remarks>This method logs the provided message and any additional context to both the logger
        /// and telemetry client.  The <paramref name="payload"/> is serialized to JSON, and the <paramref
        /// name="additionalData"/> is  appended to the log message and tracked as telemetry properties.</remarks>
        /// <param name="message">The main message to log. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="payload">An optional object representing additional context or data to include in the log.  If provided, it will be
        /// serialized to JSON and included in the log.</param>
        /// <param name="additionalData">A dictionary containing key-value pairs of additional data to include in the log.  Keys represent the data
        /// labels, and values represent the corresponding data.  If <see langword="null"/>, no additional data will be
        /// included.</param>
        public void LogInformation(string message, T payload, Dictionary<string, object> additionalData)
        {
            ArgumentNullException.ThrowIfNull(message);

            var properties = new Dictionary<string, string>
        {
            { "Message", message }
        };

            if (payload != null)
            {
                properties.Add("InputPayload", JsonConvert.SerializeObject(payload));
            }

            var additionalDataSb = new StringBuilder();
            if (additionalData != null)
            {
                additionalDataSb.AppendLine("  Additional data:");

                foreach (var item in additionalData)
                {
                    // Add to addtiona data string for log warning
                    additionalDataSb.AppendLine($" {item.Key}: {item.Value};");
                    // Add properties for tracking event telemetry
                    properties.Add(item.Key, item.Value?.ToString() ?? "");
                }
                additionalDataSb.AppendLine(" End of additional data.");
            }

            _logger.LogInformation(message + additionalDataSb.ToString());
            _telemetryClient.TrackTrace("Information", properties);
        }
    }
}
