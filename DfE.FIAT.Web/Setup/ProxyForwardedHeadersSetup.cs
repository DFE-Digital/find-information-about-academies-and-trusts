using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.HttpOverrides;

namespace DfE.FIAT.Web.Setup;

[ExcludeFromCodeCoverage]
public static class ProxyForwardedHeadersSetup
{
    public static void UseForwardedHeadersForProxy(WebApplication app)
    {
        // Ensure we do not lose X-Forwarded-* Headers when behind a Proxy
        var forwardOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All,
            RequireHeaderSymmetry = false
        };
        forwardOptions.KnownNetworks.Clear();
        forwardOptions.KnownProxies.Clear();
        app.UseForwardedHeaders(forwardOptions);
    }
}
