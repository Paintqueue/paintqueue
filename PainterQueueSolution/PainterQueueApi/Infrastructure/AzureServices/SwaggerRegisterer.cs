using Microsoft.OpenApi.Models;
using System.Reflection;

namespace PainterQueueApi.Infrastructure.AzureServices
{
    /// <summary>
    /// Provides methods to configure and enable Swagger for API documentation in a web application.
    /// </summary>
    /// <remarks>This class includes methods to register Swagger services and configure the Swagger UI for an
    /// ASP.NET Core application. Use <see cref="Initialize(WebApplicationBuilder)"/> to add Swagger services during
    /// application startup, and <see cref="ConfigureApiDocumentation(WebApplication)"/> to enable Swagger middleware and UI in the
    /// application pipeline.</remarks>
    public class SwaggerRegisterer
    {
        /// <summary>
        /// Configures Swagger services for the application.
        /// </summary>
        /// <remarks>This method adds Swagger generation services to the application's dependency
        /// injection container. It also configures Swagger to include API documentation from XML comments.</remarks>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application.</param>
        public static void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Paint Queue App", Version = "v1" });

                // Add documentation via C# XML Comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Configures the application to use Swagger for API documentation and the Swagger UI for interactive API
        /// exploration.
        /// </summary>
        /// <remarks>This method enables the Swagger middleware to generate and serve the OpenAPI
        /// specification for the application. It also configures the Swagger UI at the root URL ("/") to allow users to
        /// explore and test the API interactively. This </remarks>
        /// <param name="app">The <see cref="WebApplication"/> instance to configure. This parameter cannot be <see langword="null"/>.</param>
        public static void ConfigureApiDocumentation(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Paint Queue App");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}
