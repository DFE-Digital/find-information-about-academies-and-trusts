namespace DfE.FindInformationAcademiesTrusts;

public class ResponseHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        SetHeaderIfEmpty(context, "X-Frame-Options", "deny");
        SetHeaderIfEmpty(context, "X-Content-Type-Options", "nosniff");
        SetHeaderIfEmpty(context, "Referrer-Policy", "no-referrer");
        SetHeaderIfEmpty(context, "X-Permitted-Cross-Domain-Policies", "none");
        SetHeaderIfEmpty(context, "X-Robots-Tag", "noindex, nofollow");
        SetHeaderIfEmpty(context, "Content-Security-Policy",
            "default-src 'self'; form-action 'self'; object-src 'none'; frame-ancestors 'none'");
        SetHeaderIfEmpty(context, "Permissions-Policy",
            "accelerometer=(),ambient-light-sensor=(),autoplay=(),battery=(),camera=(),display-capture=(),document-domain=(),encrypted-media=(),fullscreen=(),gamepad=(),geolocation=(),gyroscope=(),layout-animations=(),legacy-image-formats=(),magnetometer=(),microphone=(),midi=(),oversized-images=(),payment=(),picture-in-picture=(),publickey-credentials-get=(),speaker-selection=(),sync-xhr=(),unoptimized-images=(),unsized-media=(),usb=(),screen-wake-lock=(),web-share=(),xr-spatial-tracking=()");
        SetHeaderIfEmpty(context, "Cross-Origin-Embedder-Policy", "require-corp");
        SetHeaderIfEmpty(context, "Cross-Origin-Opener-Policy", "same-origin");
        SetHeaderIfEmpty(context, "Cross-Origin-Resource-Policy", "same-origin");

        return _next(context);
    }

    private static void SetHeaderIfEmpty(HttpContext context, string headerName, string value)
    {
        if (context.Response.Headers.ContainsKey(headerName))
        {
            return;
        }

        context.Response.Headers[headerName] = value;
    }
}
