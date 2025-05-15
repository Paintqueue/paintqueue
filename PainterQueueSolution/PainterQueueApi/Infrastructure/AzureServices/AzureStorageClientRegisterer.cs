using PainterQueueApi.Infrastructure.Interfaces;

namespace PainterQueueApi.Infrastructure.AzureServices
{
    /// <summary>
    /// Provides methods to configure and register storage-related services in a web application.
    /// </summary>
    /// <remarks>This class is responsible for initializing and validating Azure Storage settings and
    /// registering the necessary storage client services into the application's dependency injection
    /// container.</remarks>
    public class AzureStorageClientRegisterer
    {
        /// <summary>
        /// Configures the storage settings and registers the storage client service for the application.
        /// </summary>
        /// <remarks>This method adds storage-related settings to the application and registers an
        /// implementation of  <see cref="IStorageClient"/> as a singleton service. Ensure that the necessary
        /// configuration  for storage is provided in the application's configuration sources.</remarks>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application.</param>
        public static void Initialize(WebApplicationBuilder builder)
        {
            var settings = AddStorageSettings(builder);

            builder.Services.AddSingleton<IStorageClient, AzureStorageClient>();
        }

        private static void Validate(AzureStorageSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "Azure Storage settings cannot be null");
            }

            if (string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty", nameof(settings.ConnectionString));
            }

            if (string.IsNullOrWhiteSpace(settings.SeedContainerName))
            {
                throw new ArgumentException("Seed container name cannot be null or empty", nameof(settings.SeedContainerName));
            }

            if (string.IsNullOrWhiteSpace(settings.RuleBlobName))
            {
                throw new ArgumentException("Rule blob name cannot be null or empty", nameof(settings.RuleBlobName));
            }
        }

        private static AzureStorageSettings AddStorageSettings(WebApplicationBuilder builder)
        {
            var settings = builder.Configuration.GetSection(key: "AzureStorageSettings").Get<AzureStorageSettings>() ?? new AzureStorageSettings();
            Validate(settings);
            builder.Services.AddSingleton(settings);

            return settings;
        }
    }

    /// <summary>
    /// Represents the configuration settings required to connect to an Azure Storage account.
    /// </summary>
    /// <remarks>This class encapsulates the connection string used to authenticate and interact with Azure
    /// Storage services. Ensure that the connection string is valid and properly formatted before using it in storage
    /// operations.</remarks>
    public class AzureStorageSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string SeedContainerName { get; set; } = string.Empty;
        public string RuleBlobName { get; set; } = string.Empty;
    }
}
