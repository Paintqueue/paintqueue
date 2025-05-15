using PainterQueueApi.Infrastructure.AzureServices;
using PainterQueueApi.Infrastructure.Interfaces;
using PainterQueueApi.Models;
using PainterQueueApi.Tools.Parsers;

namespace PainterQueueApi.Infrastructure.Database
{
    /// <summary>
    /// Provides methods to initialize and seed the database for the Paint Queue application.
    /// </summary>
    /// <remarks>This class contains functionality to ensure the database is created and populated with
    /// initial data. The <see cref="SeedAsync(PaintQueueDbContext, IStorageClient, AzureStorageSettings)"/> method
    /// should be called during application startup to ensure the database is properly initialized.</remarks>
    public static class PaintQueueDbInitializer
    {
        /// <summary>
        /// Seeds the database with initial data if the database already exists.
        /// </summary>
        /// <remarks>If the database is newly created, no seeding is performed, and the method returns
        /// immediately. Otherwise, the method populates the database with predefined rules using the provided storage
        /// client and settings.</remarks>
        /// <param name="context">The database context used to access and modify the database.</param>
        /// <param name="storageClient">The storage client used to interact with the storage service.</param>
        /// <param name="storageSettings">The settings required to configure the storage service.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task SeedAsync(PaintQueueDbContext context, IStorageClient storageClient, AzureStorageSettings storageSettings, ICustomTelemetryClient<Rule> logger)
        {

            if (!context.Rules.Any())
            {
                await AddRules(context, storageClient, storageSettings, logger);
            }
        }


        private static async Task AddRules(PaintQueueDbContext context, IStorageClient storageClient, AzureStorageSettings storageSettings, ICustomTelemetryClient<Rule> logger)
        {
            try
            {
                // Get file from storage
                var file = await storageClient.GetBlob(storageSettings.SeedContainerName, storageSettings.RuleBlobName);

                if (file == null)
                {
                    throw new Exception("Failed to retrieve rules file from storage.");
                }

                // Parse json file
                var rules = await CustomJsonParser.ParseStreamAsync<Rule>(file.FileStream);

                if (rules == null || !rules.Any())
                {
                    throw new Exception("Failed to parse rules from the file.");
                }

                // Add rules to the database
                foreach (var rule in rules)
                {
                    context.Rules.Add(rule);
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "Error adding rules to the database.");

                throw new Exception("Failed to add rules to the database.", ex);
            }
        }
    }
}
