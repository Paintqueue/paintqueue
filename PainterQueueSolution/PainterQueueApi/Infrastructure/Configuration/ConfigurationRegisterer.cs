namespace PainterQueueApi.Infrastructure.Configuration
{
    /// <summary>
    /// Configuring application settings.
    /// </summary>
    /// <remarks>This class is responsible for setting up the application's configuration sources, including
    /// JSON files, user secrets, and environment variables.</remarks>
    public class ConfigurationRegisterer
    {
        /// <summary>
        /// Configures the application's configuration sources.
        /// </summary>
        /// <remarks>This method adds configuration sources to the application, including: <list
        /// type="bullet"> <item><description>A required JSON file named <c>appsettings.json</c> with support for
        /// reloading on changes.</description></item> <item><description>User secrets for the <c>Program</c>
        /// class.</description></item> <item><description>Environment variables.</description></item> </list></remarks>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application.</param>
        public static void Initialize(WebApplicationBuilder builder)
        {
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();
        }
    }
}
