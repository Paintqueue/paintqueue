using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace PainterQueueApi.Infrastructure.AzureServices
{
    /// <summary>
    /// Allows registering Microsoft Identity services with the ASP.NET Core dependency injection.
    /// container.
    /// </summary>
    /// <remarks>This class configures authentication and authorization for an ASP.NET Core
    /// application using Microsoft Identity Web. It reads configuration settings from the "AzureAd" section of the
    /// application's configuration and validates them before registering the necessary services.</remarks>
    public class MicrosoftIdentityServiceRegisterer
    {
        /// <summary>
        /// Configures authentication and authorization for the application using Microsoft Identity Web.
        /// </summary>
        /// <remarks>This method sets up OpenID Connect authentication with Microsoft Identity Web, using
        /// settings retrieved from the application's configuration. It also configures authorization policies for the
        /// application.</remarks>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application's services and middleware.</param>
        public static void Initialize(WebApplicationBuilder builder)
        {
            var settings = AddSettings(builder);

            builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(options =>
                {
                    options.Instance = settings.Instance;
                    options.Domain = settings.Domain;
                    options.TenantId = settings.TenantId;
                    options.ClientId = settings.ClientId;
                    options.CallbackPath = settings.CallbackPath;
                    options.SignUpSignInPolicyId = settings.SignUpSignInPolicyId;
                    options.SignedOutCallbackPath = settings.SignedOutCallbackPath;
                    options.ResetPasswordPolicyId = settings.ResetPasswordPolicyId;
                    options.EditProfilePolicyId = settings.EditProfilePolicyId;
                });

            builder.Services.AddAuthorization(options =>
            {
                //options.FallbackPolicy = options.DefaultPolicy;
            });
        }

        private static void Validate(MicrosoftIdentityServiceSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "Identity settings cannot be null");
            }

            if (string.IsNullOrWhiteSpace(settings.Instance))
            {
                throw new ArgumentException("Instance cannot be null or empty", nameof(settings.Instance));
            }

            if (string.IsNullOrWhiteSpace(settings.Domain))
            {
                throw new ArgumentException("Domain cannot be null or empty", nameof(settings.Domain));
            }

            if (string.IsNullOrWhiteSpace(settings.TenantId))
            {
                throw new ArgumentException("TenantId cannot be null or empty", nameof(settings.TenantId));
            }

            if (string.IsNullOrWhiteSpace(settings.ClientId))
            {
                throw new ArgumentException("ClientId cannot be null or empty", nameof(settings.ClientId));
            }

            if (string.IsNullOrWhiteSpace(settings.CallbackPath))
            {
                throw new ArgumentException("CallbackPath cannot be null or empty", nameof(settings.CallbackPath));
            }

            if (string.IsNullOrWhiteSpace(settings.SignUpSignInPolicyId))
            {
                throw new ArgumentException("SignUpSignInPolicyId cannot be null or empty", nameof(settings.SignUpSignInPolicyId));
            }

            if (string.IsNullOrWhiteSpace(settings.SignedOutCallbackPath))
            {
                throw new ArgumentException("SignedOutCallbackPath cannot be null or empty", nameof(settings.SignedOutCallbackPath));
            }

            if (string.IsNullOrWhiteSpace(settings.ResetPasswordPolicyId))
            {
                throw new ArgumentException("ResetPasswordPolicyId cannot be null or empty", nameof(settings.ResetPasswordPolicyId));
            }

            if (string.IsNullOrWhiteSpace(settings.EditProfilePolicyId))
            {
                throw new ArgumentException("EditProfilePolicyId cannot be null or empty", nameof(settings.EditProfilePolicyId));
            }
        }

        private static MicrosoftIdentityServiceSettings AddSettings(WebApplicationBuilder builder)
        {
            var settings = builder.Configuration.GetSection(key: "AzureAd").Get<MicrosoftIdentityServiceSettings>() ?? new MicrosoftIdentityServiceSettings();
            Validate(settings);
            builder.Services.AddSingleton(settings);
            return settings;
        }
    }

    /// <summary>
    /// Represents the configuration settings required to integrate with Microsoft Identity services.
    /// </summary>
    /// <remarks>This class encapsulates the necessary parameters for configuring authentication and
    /// authorization with Microsoft Identity services.</remarks>
    public class MicrosoftIdentityServiceSettings
    {
        public string Instance { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string CallbackPath { get; set; } = string.Empty;
        public string SignUpSignInPolicyId { get; set; } = string.Empty;
        public string SignedOutCallbackPath { get; set; } = string.Empty;
        public string ResetPasswordPolicyId { get; set; } = string.Empty;
        public string EditProfilePolicyId { get; set; } = string.Empty;
        public bool EnablePiiLogging { get; set; }
    }
}

