using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DfE.FIAT.Web.Options;
using Microsoft.FeatureManagement;

namespace DfE.FIAT.Web.Setup;

[ExcludeFromCodeCoverage]
public static class ConfigurationVariables
{
    public static void BindConfigurationVariables(WebApplicationBuilder builder)
    {
        if (builder.Environment.IsLocalDevelopment())
            builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

        builder.Services.AddOptions<TestOverrideOptions>()
            .Bind(builder.Configuration.GetSection(TestOverrideOptions.ConfigurationSection));
        builder.Services.AddOptions<ApplicationInsightsOptions>()
            .Bind(builder.Configuration.GetSection(ApplicationInsightsOptions.ConfigurationSection));
        builder.Services.AddOptions<NotificationBannerOptions>()
            .Bind(builder.Configuration.GetSection(NotificationBannerOptions.ConfigurationSection));

        builder.Services.AddFeatureManagement();
    }
}
