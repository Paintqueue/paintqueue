using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;

namespace PainterQueueApi.Infrastructure.AzureServices
{
    /// <summary>
    /// Configures and registers Application Insights telemetry for the specified web application builder.
    /// </summary>
    /// <remarks>This method sets up Application Insights telemetry by: <list type="bullet">
    /// <item><description>Loading configuration settings from the "ApplicationInsightsSettings" section of the
    /// application's configuration.</description></item> <item><description>Validating the required settings, such as
    /// the API key, connection string, and adaptive sampling configuration.</description></item>
    /// <item><description>Registering Application Insights telemetry services with the specified options, including
    /// enabling adaptive sampling and setting the connection string.</description></item>
    /// <item><description>Configuring the <see cref="QuickPulseTelemetryModule"/> with the provided API key for live
    /// metrics streaming.</description></item> </list></remarks>
    public class ApplicationInsightsRegisterer
    {
        /// <summary>
        /// Configures Application Insights telemetry for the specified web application builder.
        /// </summary>
        /// <remarks>This method sets up Application Insights telemetry with options derived from the
        /// application's settings. It enables adaptive sampling and configures the connection string and authentication
        /// API key for telemetry modules.</remarks>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application.</param>
        public static void Initialize(WebApplicationBuilder builder)
        {
            var settings = ConfigureSettings(builder);

            var options = new ApplicationInsightsServiceOptions()
            {
                EnableAdaptiveSampling = settings.EnableAdaptiveSampling,
                ConnectionString = settings.ConnectionString,
            };

            builder.Services.AddApplicationInsightsTelemetry(options);

            builder.Services.ConfigureTelemetryModule<QuickPulseTelemetryModule>((module, _) =>
            {
                module.AuthenticationApiKey = settings.ApiKey;
            });
        }

        private static ApplicationInsightsSettings ConfigureSettings(WebApplicationBuilder builder)
        {
            var settings = builder.Configuration.GetSection(key: "ApplicationInsightsSettings").Get<ApplicationInsightsSettings>() ?? new ApplicationInsightsSettings();
            
            Validate(settings);
            
            return settings;
        }

        private static void Validate(ApplicationInsightsSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "Application Insights settings cannot be null");
            }
            
            if (string.IsNullOrWhiteSpace(settings.ApiKey))
            {
                throw new ArgumentException("API key cannot be null or empty", nameof(settings.ApiKey));
            }
            
            if (string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty", nameof(settings.ConnectionString));
            }

            if (settings.EnableAdaptiveSampling == false)
            {
                throw new ArgumentException("Adaptive sampling must be enabled", nameof(settings.EnableAdaptiveSampling));
            }   
        }
    }

    /// <summary>
    /// Represents the configuration settings for integrating with Azure Application Insights.
    /// </summary>
    /// <remarks>This class provides properties to configure the connection to Application Insights, 
    /// including the API key, connection string, and adaptive sampling settings.</remarks>
    public class ApplicationInsightsSettings
    {
        public string ApiKey { get; set; } = "";
        public string ConnectionString { get; set; } = "";
        public bool EnableAdaptiveSampling { get; set; } = true;
    }
}
