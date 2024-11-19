namespace DfE.FIAT.Web;

public class ResponseHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        SetHeaderIfEmpty(context, "X-Robots-Tag", "noindex, nofollow");
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
