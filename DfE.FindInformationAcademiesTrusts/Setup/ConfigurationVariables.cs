using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Azure.Identity;
using DfE.FindInformationAcademiesTrusts.Options;
using Microsoft.FeatureManagement;
using Serilog;

namespace DfE.FindInformationAcademiesTrusts.Setup;

[ExcludeFromCodeCoverage]
public static class ConfigurationVariables
{
    public static void BindConfigurationVariables(WebApplicationBuilder builder)
    {
        if (builder.Environment.IsLocalDevelopment())
            builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
        
        // Retrieve the connection string
        string? appConfigConnectionString = builder.Configuration.GetConnectionString("AppConfig");

        // Load App Configuration and Feature Flags from Azure
        if (!string.IsNullOrEmpty(appConfigConnectionString))
        {
            // Check to see if a Managed Identity has been set
            string? azureClientId = builder.Configuration.GetSection("AZURE_CLIENT_ID").Value;

            // Register App Configuration
            builder.Configuration.AddAzureAppConfiguration(options =>
                    options.Connect(
                        new Uri(appConfigConnectionString),
                        new ManagedIdentityCredential(azureClientId)
                    ).UseFeatureFlags(),
                true
            );
        }
        else
        {
            Log.Warning("AppConfig not found in configuration, will not add Azure App Config");
        }

        builder.Services.AddOptions<TestOverrideOptions>()
            .Bind(builder.Configuration.GetSection(TestOverrideOptions.ConfigurationSection));
        builder.Services.AddOptions<ApplicationInsightsOptions>()
            .Bind(builder.Configuration.GetSection(ApplicationInsightsOptions.ConfigurationSection));
        builder.Services.AddOptions<NotificationBannerOptions>()
            .Bind(builder.Configuration.GetSection(NotificationBannerOptions.ConfigurationSection));

        builder.Services.AddFeatureManagement();
    }
}
