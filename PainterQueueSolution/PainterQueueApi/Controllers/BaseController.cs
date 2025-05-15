using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using PainterQueueApi.Infrastructure.CustomServices;
using PainterQueueApi.Infrastructure.Interfaces;

namespace HW4NoteKeeperEx2.Controllers
{
    /// <summary>
    /// Provides a base controller class for handling HTTP responses with custom telemetry logging.
    /// </summary>
    /// <remarks>This abstract class extends <see cref="ControllerBase"/> and provides utility methods for
    /// creating HTTP responses with integrated telemetry logging. It supports logging informational messages, warnings,
    /// and exceptions, along with optional payloads and additional data. Derived controllers should these methods to
    /// standardize response handling and logging behavior.</remarks>
    /// <typeparam name="T">The type of the payload or object being logged by the telemetry client.</typeparam>
    public abstract class BaseController<T>: ControllerBase
    {
        protected readonly ICustomTelemetryClient<T> _tracker;
        public BaseController(ICustomTelemetryClient<T> tracker)
        {
            _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
        }

        #region Success 200 Responses
        /// <summary>
        /// Creates a custom response that logs additional data for a successful request.
        /// </summary>
        /// <typeparam name="T">Payload type</typeparam>
        /// <param name="tracker">Logger</param>
        /// <param name="title">title of log information</param>
        /// <param name="details">details of log information</param>
        /// <param name="additionalData">Additional information</param>
        /// <returns>Ok response</returns>
        protected IActionResult OkLogCustomResponse(string title, string details, Dictionary<string, object>? additionalData = null)
        {
            return OkLogCustom200Response(title, details, default, additionalData);
        }

        /// <summary>
        /// Creates a custom response that logs payload and additional data for a successful request.
        /// </summary>
        /// <typeparam name="T">Payload type</typeparam>
        /// <param name="tracker">Logger</param>
        /// <param name="title">title of log information</param>
        /// <param name="details">details of log information</param>
        /// <param name="payload">Payload to log</param>
        /// <param name="additionalData">Additional information</param>
        /// <returns>Ok response</returns>
        protected IActionResult OkLogCustom200Response(string title, string details, T? payload, Dictionary<string, object>? additionalData = null)
        {
            LogInformationCustomResponse(title, StatusCodes.Status200OK, details, payload, additionalData);

            return Ok();
        }

        /// <summary>
        /// Creates a custom response that additional data for a successful request.
        /// </summary>
        /// <typeparam name="T">Payload type</typeparam>
        /// <param name="tracker">Logger</param>
        /// <param name="title">title of log information</param>
        /// <param name="details">details of log information</param>
        /// <param name="additionalData">Additional information</param>
        /// <returns>Created response</returns>
        protected IActionResult CreatedLogCustomReponse(string title, string details, Dictionary<string, object>? additionalData = null)
        {
            return CreatedLogCustomReponse(title, details, default, additionalData);
        }

        /// <summary>
        /// Creates a custom response that logs payload and additional data for a successful request.
        /// </summary>
        /// <typeparam name="T">Payload type</typeparam>
        /// <param name="tracker">Logger</param>
        /// <param name="title">title of log information</param>
        /// <param name="details">details of log information</param>
        /// <param name="payload">Payload to log</param>
        /// <param name="additionalData">Additional information</param>
        /// <returns>Created response</returns>
        protected IActionResult CreatedLogCustomReponse(string title, string details, T? payload, Dictionary<string, object>? additionalData = null)
        {
            LogInformationCustomResponse(title, StatusCodes.Status201Created, details, payload, additionalData);

            return Created();
        }

        /// <summary>
        /// Creates a custom response that logs payload and additional data for a successful request.
        /// </summary>
        /// <typeparam name="T">Payload type</typeparam>
        /// <param name="tracker">Logger</param>
        /// <param name="title">title of log information</param>
        /// <param name="details">details of log information</param>
        /// <param name="additionalData">Additional information</param>
        /// <returns>No content response</returns>
        protected IActionResult NoContentLogCustomResponse(string title, string details, Dictionary<string, object>? additionalData = null)
        {
            return NoContentLogCustomResponse(title, details, default, additionalData);
        }

        /// <summary>
        /// Creates a custom response that logs payload and additional data for a successful request.
        /// </summary>
        /// <typeparam name="T">Payload type</typeparam>
        /// <param name="tracker">Logger</param>
        /// <param name="title">title of log information</param>
        /// <param name="details">details of log information</param>
        /// <param name="payload">Payload to log</param>
        /// <param name="additionalData">Additional information</param>
        /// <returns>No content response</returns>
        protected IActionResult NoContentLogCustomResponse(string title, string details, T? payload, Dictionary<string, object>? additionalData = null)
        {
            LogInformationCustomResponse(title, StatusCodes.Status204NoContent, details, payload, additionalData);

            return NoContent();
        }

        /// <summary>
        /// Creates a custom response that logs warning with additional data for a successful request.
        /// </summary>
        /// <typeparam name="T">Payload type</typeparam>
        /// <param name="tracker">Logger</param>
        /// <param name="title">title of log information</param>
        /// <param name="details">details of log information</param>
        /// <param name="additionalData">Additional information</param>
        /// <returns>No content response</returns>
        protected IActionResult NoContentWarnCustomResponse(string title, string details, Dictionary<string, object>? additionalData = null)
        {
            return NoContentWarnCustomResponse(title, details, default, additionalData);
        }


        /// <summary>
        /// Creates a custom response that logs warning with additional data for a successful request.
        /// </summary>
        /// <typeparam name="T">Payload type</typeparam>
        /// <param name="tracker">Logger</param>
        /// <param name="title">title of log information</param>
        /// <param name="details">details of log information</param>
        /// <param name="payload">Payload to log</param>
        /// <param name="additionalData">Additional information</param>
        /// <returns>No content response</returns>
        protected IActionResult NoContentWarnCustomResponse(string title, string details, T? payload, Dictionary<string, object>? additionalData = null)
        {
            LogWarnCustomResponse(title, StatusCodes.Status204NoContent, details, payload, additionalData);

            return NoContent();
        }

        #endregion

        #region Client 400 Responses

        /// <summary>
        /// Creates a custom response with ProblemDetails for a bad request (no exception).
        /// </summary>
        /// <typeparam name="T">Object for tracker</typeparam>
        /// <param name="tracker">Aplication insights tracker</param>
        /// <param name="title">Title for problem details</param>
        /// <param name="details">Details for problem details</param>
        /// <param name="additionalData">Optional additional data</param>
        /// <returns></returns>
        protected ObjectResult BadRequestCustomResponse(string title, string details, Dictionary<string, object>? additionalData = null)
        {
            return BadRequestCustomResponse(title, details, default, additionalData);
        }

        /// <summary>
        /// Creates a custom response with ProblemDetails for a bad request (no excepton).
        /// </summary>
        /// <typeparam name="T">Object for tracker</typeparam>
        /// <param name="tracker">Aplication insights tracker</param>
        /// <param name="title">Title for problem details</param>
        /// <param name="details">Details for problem details</param>
        /// <param name="payload">Optional Object that caused error</param>
        /// <param name="additionalData">Optional additional data</param>
        /// <returns></returns>
        protected ObjectResult BadRequestCustomResponse(string title, string details, T? payload, Dictionary<string, object>? additionalData = null)
        {
            return DefaultClientCustomResponse(title, StatusCodes.Status400BadRequest, details, payload, additionalData);
        }

        /// <summary>
        /// Creates a custom response with ProblemDetails for a bad request exception.
        /// </summary>
        /// <typeparam name="T">Object type for tracker</typeparam>
        /// <param name="tracker">Application insights tracker</param>
        /// <param name="payload">payload sent on request</param>
        /// <param name="ex">exception</param>
        /// <param name="title">title for the problem details</param>
        /// <returns>Object for the response that includes status code and problem details</returns>>
        protected ObjectResult BadRequestExceptionCustomResponse(T payload, Exception ex, string title)
        {
            return DefualtExceptionCustomResponse(payload, ex, title, StatusCodes.Status400BadRequest);
        }


        /// <summary>
        /// Creates a custom response with ProblemDetails for a forbidden (non-exception).
        /// </summary>
        /// <typeparam name="T">Object for tracker</typeparam>
        /// <param name="tracker">Aplication insights tracker</param>
        /// <param name="title">Title for problem details</param>
        /// <param name="details">Details for problem details</param>
        /// <param name="additionalData">Optional additional data</param>
        /// <returns>Object for the response that includes status code and problem details</returns>
        protected ObjectResult ForbiddenCustomResponse(string title, string details, Dictionary<string, object>? additionalData = null)
        {
            return ForbiddenCustomResponse(title, details, default, additionalData);
        }

        /// <summary>
        /// Creates a custom response with ProblemDetails for a forbidden (non-exception).
        /// </summary>
        /// <typeparam name="T">Object for tracker</typeparam>
        /// <param name="tracker">Aplication insights tracker</param>
        /// <param name="title">Title for problem details</param>
        /// <param name="details">Details for problem details</param>
        /// <param name="payload">Optional Object that caused error</param>
        /// <param name="additionalData">Optional additional data</param>
        /// <returns>Object for the response that includes status code and problem details</returns>
        protected ObjectResult ForbiddenCustomResponse(string title, string details, T? payload, Dictionary<string, object>? additionalData = null)
        {
            return DefaultClientCustomResponse(title, StatusCodes.Status403Forbidden, details, payload, additionalData);
        }

        /// <summary>
        /// Creates a custom response with ProblemDetails for a forbidden exception.
        /// </summary>
        /// <typeparam name="T">Object type for tracker</typeparam>
        /// <param name="tracker">Application insights tracker</param>
        /// <param name="payload">payload sent on request</param>
        /// <param name="ex">exception</param>
        /// <param name="title">title for the problem details</param>
        /// <returns>Object for the response that includes status code and problem details</returns>
        protected ObjectResult ForbiddenExceptionCustomResponse(T payload, Exception ex, string title)
        {
            return DefualtExceptionCustomResponse(payload, ex, title, StatusCodes.Status403Forbidden);
        }

        /// <summary>
        /// Creates a custom response with ProblemDetails for a not found (non exception).
        /// </summary>
        /// <typeparam name="T">Object for tracker</typeparam>
        /// <param name="tracker">Aplication insights tracker</param>
        /// <param name="title">Title for problem details</param>
        /// <param name="details">Details for problem details</param>
        /// <param name="additionalData">Optional additional data</param>
        /// <returns>Object for the response that includes status code and problem details</returns>
        protected ObjectResult NotFoundCustomResponse(string title, string details, Dictionary<string, object>? additionalData = null)
        {
            return NotFoundCustomResponse(title, details, default, additionalData);
        }

        /// <summary>
        /// Creates a custom response with ProblemDetails for a not found (non exception).
        /// </summary>
        /// <typeparam name="T">Object for tracker</typeparam>
        /// <param name="tracker">Aplication insights tracker</param>
        /// <param name="title">Title for problem details</param>
        /// <param name="details">Details for problem details</param>
        /// <param name="payload">Optional Object that caused error</param>
        /// <param name="additionalData">Optional additional data</param>
        /// <returns>Object for the response that includes status code and problem details</returns>
        protected ObjectResult NotFoundCustomResponse(string title, string details, T? payload, Dictionary<string, object>? additionalData = null)
        {
            return DefaultClientCustomResponse(title, StatusCodes.Status404NotFound, details, payload, additionalData);
        }

        /// <summary>
        /// Creates a custom response with ProblemDetails for a not found exception.
        /// </summary>
        /// <typeparam name="T">Object type for tracker</typeparam>
        /// <param name="tracker">Application insights tracker</param>
        /// <param name="payload">payload sent on request</param>
        /// <param name="ex">exception</param>
        /// <param name="title">title for the problem details</param>
        /// <returns>Object for the response that includes status code and problem details</returns>
        protected ObjectResult NotFoundExceptionCustomResponse(T payload, Exception ex, string title)
        {
            return DefualtExceptionCustomResponse(payload, ex, title, StatusCodes.Status404NotFound);
        }

        #endregion

        #region Server 500 Responses

        /// <summary>
        /// Creates a custom response with ProblemDetails for an internal server error for a reuqest with a payload.
        /// </summary>
        /// <typeparam name="T">Object type for tracker</typeparam>
        /// <param name="tracker">Application insights tracker</param>
        /// <param name="payload">payload sent on request</param>_noteRepository
        /// <param name="ex">exception</param>
        /// <returns>Object for the response that includes status code and problem details</returns>
        protected ObjectResult InternalServerErrorExceptionCustomResponse(T payload, Exception ex)
        {
            return DefualtExceptionCustomResponse(payload, ex, "Internal Server Error", StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Creates a custom response with ProblemDetails for an internal server error exception for a request without a payload.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tracker"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected ObjectResult InternalServerErrorExceptionCustomResponse(Exception ex)
        {
            return DefualtExceptionCustomResponse(ex, "Internal Server Error", StatusCodes.Status500InternalServerError);
        }

        #endregion

        #region Default Responses

        /// <summary>
        /// Defualt a custom response with ProblemDetails to be inherited by other exception responses. Includes payload.
        /// </summary>
        /// <typeparam name="T">Object type for tracker</typeparam>
        /// <param name="tracker">Application insights tracker</param>
        /// <param name="payload">payload sent on request</param>
        /// <param name="ex">exception</param>
        /// <param name="title">title for the problem details</param>
        /// <param name="statusCode">status code for the response</param>
        /// <returns>Object for the response that includes status code and problem details</returns>
        private ObjectResult DefualtExceptionCustomResponse(T payload, Exception ex, string title, int statusCode)
        {
            LogExceptionCustomResponse(ex, title, payload, null);

            return DefaultExceptionCustomResponse(ex, title, statusCode);
        }

        /// <summary>
        /// Defualt a custom response with ProblemDetails to be inherited by other exception responses.  No payload.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tracker">logger</param>
        /// <param name="ex">exception</param>
        /// <param name="title">title</param>
        /// <param name="statusCode">status code</param>
        /// <returns></returns>
        private ObjectResult DefualtExceptionCustomResponse(Exception ex, string title, int statusCode)
        {
            LogExceptionCustomResponse(ex, title, default, null);

            return DefaultExceptionCustomResponse(ex, title, statusCode);
        }

        /// <summary>
        ///  Defualt a custom response with ProblemDetails to be inherited by other exception responses.
        /// </summary>
        /// <param name="ex">exception</param>
        /// <param name="title">title for the problem details</param>
        /// <param name="statusCode">status code for the response</param>
        /// <returns>Object for the response that includes status code and problem details</returns>
        private ObjectResult DefaultExceptionCustomResponse(Exception ex, string title, int statusCode)
        {
            return DefaultClientServerProblemDetailsCustomResponse(title, statusCode, ex.Message);
        }

        /// <summary>
        /// Creates a custom response with ProblemDetails. Client reponse requests send this response and log as warnings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tracker">logger</param>
        /// <param name="title">title</param>
        /// <param name="statusCode">status code</param>
        /// <param name="detail">details</param>
        /// <param name="payload">payload</param>
        /// <param name="additionalData"></param>
        /// <returns></returns>
        private ObjectResult DefaultClientCustomResponse(string title, int statusCode, string detail, T? payload, Dictionary<string, object>? additionalData = null)
        {
            LogWarnCustomResponse(title, statusCode, detail, payload, additionalData);

            return DefaultClientServerProblemDetailsCustomResponse(title, statusCode, detail);
        }

        /// <summary>
        /// Creates a custom response with ProblemDetails. Exceptions and bad client/server requests send this response.
        /// </summary>
        /// <param name="title">Title of the reponse</param>
        /// <param name="statusCode">Status code</param>
        /// <param name="detail">details of message</param>
        /// <returns>returns problem details</returns>
        private ObjectResult DefaultClientServerProblemDetailsCustomResponse(string title, int statusCode, string detail)
        {
            var problemDetails = new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Detail = detail
            };

            return StatusCode(statusCode, problemDetails);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Logs an informational message with a custom telemetry client, including optional details such as a payload
        /// and additional data.
        /// </summary>
        /// <remarks>The method determines the appropriate overload of the telemetry client's logging
        /// method to call based on the presence of the <paramref name="payload"/> and <paramref name="additionalData"/>
        /// parameters.</remarks>
        /// <typeparam name="T">The type of the payload to be logged.</typeparam>
        /// <param name="tracker">The telemetry client used to log the information. Cannot be null.</param>
        /// <param name="title">The title or summary of the log entry. This parameter is not used directly in the method.</param>
        /// <param name="statusCode">The status code associated with the log entry. This parameter is not used directly in the method.</param>
        /// <param name="detail">The detailed message to log. Cannot be null or empty.</param>
        /// <param name="payload">An optional payload of type <typeparamref name="T"/> to include in the log entry. Can be null.</param>
        /// <param name="additionalData">An optional dictionary of additional data to include in the log entry. Can be null.</param>
        private void LogInformationCustomResponse(string title, int statusCode, string detail, T? payload, Dictionary<string, object>? additionalData = null)
        {
            additionalData ??= new Dictionary<string, object>();
            additionalData.Add("StatusCode", statusCode);
            additionalData.Add("Title", title);

            if (payload is not null && additionalData is not null)
            {
                _tracker.LogInformation(detail, payload, additionalData);
            }
            else if (payload is not null)
            {
                _tracker.LogInformation(detail, payload);
            }
            else if (additionalData is not null)
            {
                _tracker.LogInformation(detail, additionalData);
            }
            else
            {
                _tracker.LogInformation(detail);
            }
        }

        /// <summary>
        /// Logs a warning message with optional payload and additional data.
        /// </summary>
        /// <remarks>This method logs a warning message using the provided detail, and optionally includes
        /// a payload and/or additional data. The behavior of the log entry depends on which parameters are provided:
        /// <list type="bullet"> <item> <description>If both <paramref name="payload"/> and <paramref
        /// name="additionalData"/> are provided, both are included in the log.</description> </item> <item>
        /// <description>If only <paramref name="payload"/> is provided, it is included in the log.</description>
        /// </item> <item> <description>If only <paramref name="additionalData"/> is provided, it is included in the
        /// log.</description> </item> <item> <description>If neither <paramref name="payload"/> nor <paramref
        /// name="additionalData"/> is provided, only the detail message is logged.</description> </item>
        /// </list></remarks>
        /// <param name="detail">The detail message to include in the warning log. This value cannot be null or empty.</param>
        /// <param name="payload">An optional payload object to include in the log. Can be null if no payload is provided.</param>
        /// <param name="additionalData">An optional dictionary of additional data to include in the log. Can be null if no additional data is
        /// provided.</param>
        private void LogWarnCustomResponse(string title, int statusCode, string detail, T? payload, Dictionary<string, object>? additionalData = null)
        {
            additionalData ??= new Dictionary<string, object>();
            additionalData.Add("StatusCode", statusCode);
            additionalData.Add("Title", title);

            if (payload is not null && additionalData is not null)
            {
                _tracker.LogWarning(detail, payload, additionalData);
            }
            else if (payload is not null)
            {
                _tracker.LogWarning(detail, payload);
            }
            else if (additionalData is not null)
            {
                _tracker.LogWarning(detail, additionalData);
            }
            else
            {
                _tracker.LogWarning(detail);
            }
        }

        /// <summary>
        /// Logs an exception along with a custom message, optional payload, and additional data.
        /// </summary>
        /// <remarks>This method determines the most appropriate overload of the logging mechanism to use
        /// based on the presence of the <paramref name="payload"/> and <paramref name="additionalData"/> parameters. If
        /// both are provided, both are included in the log. If neither is provided, only the exception and message are
        /// logged.</remarks>
        /// <param name="ex">The exception to be logged. Cannot be <see langword="null"/>.</param>
        /// <param name="message">A custom message providing additional context for the exception. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="payload">An optional payload of type <typeparamref name="T"/> that provides additional information about the
        /// exception context. Can be <see langword="null"/>.</param>
        /// <param name="additionalData">An optional dictionary containing additional key-value pairs to include in the log. Can be <see
        /// langword="null"/>.</param>
        private void LogExceptionCustomResponse(Exception ex, string message, T? payload, Dictionary<string, object>? additionalData = null)
        {
            if (payload is not null && additionalData is not null)
            {
                _tracker.LogException(ex, payload, additionalData, message);
            }
            else if (payload is not null)
            {
                _tracker.LogException(ex, payload, message);
            }
            else if (additionalData is not null)
            {
                _tracker.LogException(ex, additionalData, message);
            }
            else
            {
                _tracker.LogException(ex, message);
            }
       }
       
        #endregion
    }
}
