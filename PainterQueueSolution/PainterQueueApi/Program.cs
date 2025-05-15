using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web.Resource;
using PainterQueueApi.Infrastructure.Configuration;
using PainterQueueApi.Infrastructure.AzureServices;
using PainterQueueApi.Infrastructure.CustomServices;
using PainterQueueApi.Infrastructure.Database;

namespace PainterQueueApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        ConfigurationRegisterer.Initialize(builder);                // Add appsettings, secrets, and environment variables
        ApplicationInsightsRegisterer.Initialize(builder);          // Add real time application Insights telemetry
        CustomRegisterer.Initialize(builder);                       // Custom telemetry client for logging to Application Insights
        MicrosoftIdentityServiceRegisterer.Initialize(builder);     // Add Microsoft Identity authentication
        AzureStorageClientRegisterer.Initialize(builder);           // Add blob storage client
        EntityFrameworkRegisterer.Initialize(builder);              // Add Entity Framework Core with SQL Server
        SwaggerRegisterer.Initialize(builder);                      // Finish swagger setup
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        await EntityFrameworkRegisterer.SeedDatabase(app);          // Seed the database with initial data
        SwaggerRegisterer.ConfigureApiDocumentation(app);           // Enable swagger UI for API documentation
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
