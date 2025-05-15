using Microsoft.EntityFrameworkCore;
using PainterQueueApi.Infrastructure.Database;
using PainterQueueApi.Infrastructure.Interfaces;
using PainterQueueApi.Models;

namespace PainterQueueApi.Infrastructure.AzureServices
{
    /// <summary>
    /// Provides methods to configure and initialize Entity Framework for the application.
    /// </summary>
    /// <remarks>This class is responsible for registering the Entity Framework DbContext with the
    /// application's dependency injection container, configuring it to use SQL Server, and seeding the database with
    /// initial data.  Seeding data should happen after the app has built.</remarks>
    public class EntityFrameworkRegisterer
    {
        /// <summary>
        /// Configures the application by registering required services and settings.
        /// </summary>
        /// <remarks>This method registers the application's database context and loads configuration
        /// settings. It retrieves the connection string for the SQL Server database from the application's
        /// configuration and sets up Entity Framework to use it.</remarks>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application.</param>
        public static void Initialize(WebApplicationBuilder builder)
        {
            // Register the DbContext with the connection string from appsettings.json  
            var settings = AddSettings(builder);

            // Configure Entity Framework to use SQL Server  
            var connectionString = builder.Configuration.GetConnectionString("SqlDatabase") ?? "";

            builder.Services.AddDbContext<PaintQueueDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        /// <summary>
        /// Seeds the database with initial data and ensures that the database schema is up to date.
        /// </summary>
        /// <remarks>This method performs the following actions: <list type="bullet">
        /// <item><description>Creates a scoped service provider to resolve required services.</description></item>
        /// <item><description>Applies any pending migrations to the database.</description></item>
        /// <item><description>Ensures the database is created if it does not already exist.</description></item>
        /// <item><description>Invokes the database initializer to seed the database with initial
        /// data.</description></item> </list> If an error occurs during migration or seeding, an exception is thrown
        /// with details about the failure.</remarks>
        /// <param name="app">The <see cref="WebApplication"/> instance used to access application services.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown if an error occurs while migrating or seeding the database. The inner exception contains additional
        /// details.</exception>
        public static async Task SeedDatabase(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<PaintQueueDbContext>();
                var storage = serviceScope.ServiceProvider.GetRequiredService<IStorageClient>();
                var settings = serviceScope.ServiceProvider.GetRequiredService<AzureStorageSettings>();
                var telemetry = serviceScope.ServiceProvider.GetRequiredService<ICustomTelemetryClient<Rule>>();

                try
                {
                    await context.Database.EnsureCreatedAsync();
                    await context.Database.MigrateAsync();
                    await PaintQueueDbInitializer.SeedAsync(context, storage, settings, telemetry);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while migrating or seeding the database.", ex);
                }
            }
        }

        private static EntityFrameworkSettings AddSettings(WebApplicationBuilder builder)
        {
            var settings = builder.Configuration.GetSection(key: "EntityFrameworkSettings").Get<EntityFrameworkSettings>() ?? new EntityFrameworkSettings();

            Validate(settings);

            return settings;
        }

        private static void Validate(EntityFrameworkSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "Entity Framework settings cannot be null");
            }
        }
    }

    /// <summary>
    /// Represents configuration settings for Entity Framework integration.
    /// </summary>
    /// <remarks>This class is intended to encapsulate settings related to the use of Entity Framework, such
    /// as connection strings, provider configurations, or other options that influence the behavior of Entity Framework
    /// in an application.</remarks>
    public class EntityFrameworkSettings
    {
    }
}
