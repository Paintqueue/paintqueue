using Microsoft.ApplicationInsights;
using PainterQueueApi.Infrastructure.Interfaces;

namespace PainterQueueApi.Infrastructure.CustomServices
{
    /// <summary>
    /// Provides a method to register a custom telemetry client implementation in the application's dependency injection
    /// container.
    /// </summary>
    /// <remarks>This class is used to configure the application's services by registering the <see
    /// cref="ICustomTelemetryClient{T}"/> interface with its implementation, <see cref="CustomTelemetryClient{T}"/>.
    /// Call <see cref="Initialize"/> during application startup to ensure the telemetry client is available for
    /// dependency injection.</remarks>
    public static class CustomRegisterer
    {
        /// <summary>
        /// Configures the application's dependency injection container by registering the custom telemetry client
        /// service.
        /// </summary>
        /// <remarks>This method registers the <see cref="ICustomTelemetryClient{T}"/> interface with its
        /// implementation <see cref="CustomTelemetryClient{T}"/> as a singleton service in the application's dependency
        /// injection container.</remarks>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application's services.</param>
        public static void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(ICustomTelemetryClient<>), typeof(CustomTelemetryClient<>)); // Should be first.
        }
    }
}
