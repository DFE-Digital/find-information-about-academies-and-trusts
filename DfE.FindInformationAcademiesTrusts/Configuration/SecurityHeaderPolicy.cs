using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Configuration;

[ExcludeFromCodeCoverage]
public static class SecurityHeaderPolicy
{
    private static readonly string[] ScriptCspUrls =
    [
        "https://js.monitor.azure.com/scripts/b/ai.2.min.js",
        "https://js.monitor.azure.com/scripts/b/ai.3.gbl.min.js",
        "https://js.monitor.azure.com/scripts/b/ext/ai.clck.2.8.18.min.js",
        "https://www.googletagmanager.com"
    ];

    private static readonly string[] ConnectCspUrls =
    [
        "https://*.in.applicationinsights.azure.com//v2/track",
        "https://*.in.applicationinsights.azure.com/v2/track",
        "https://js.monitor.azure.com/scripts/b/ai.config.1.cfg.json",
        "https://*.google-analytics.com"
    ];

    public static HeaderPolicyCollection GetSecurityHeaderPolicies()
    {
        return new HeaderPolicyCollection()
            .AddFrameOptionsDeny()
            .AddXssProtectionDisabled()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .RemoveServerHeader()
            .AddContentSecurityPolicy(cspBuilder =>
            {
                cspBuilder.AddDefaultSrc().Self();
                cspBuilder.AddScriptSrc()
                    .Self()
                    .UnsafeInline()
                    .WithNonce()
                    .From(ScriptCspUrls);
                cspBuilder.AddConnectSrc()
                    .Self()
                    .From(ConnectCspUrls);
                cspBuilder.AddObjectSrc().None();
                cspBuilder.AddBlockAllMixedContent();
                cspBuilder.AddImgSrc().Self();
                cspBuilder.AddFormAction().Self();
                cspBuilder.AddFontSrc().Self();
                cspBuilder.AddStyleSrc().Self();
                cspBuilder.AddBaseUri().Self();
                cspBuilder.AddFrameAncestors().None();
            })
            .AddPermissionsPolicy(builder =>
            {
                builder.AddAccelerometer().None();
                builder.AddAutoplay().None();
                builder.AddCamera().None();
                builder.AddEncryptedMedia().None();
                builder.AddFullscreen().All();
                builder.AddGeolocation().None();
                builder.AddGyroscope().None();
                builder.AddMagnetometer().None();
                builder.AddMicrophone().None();
                builder.AddMidi().None();
                builder.AddPayment().None();
                builder.AddPictureInPicture().None();
                builder.AddSyncXHR().None();
                builder.AddUsb().None();
            })
            .AddCrossOriginOpenerPolicy(builder => { builder.SameOrigin(); })
            .AddCrossOriginEmbedderPolicy(builder => { builder.RequireCorp(); })
            .AddCrossOriginResourcePolicy(builder => { builder.SameOrigin(); });
    }
}
