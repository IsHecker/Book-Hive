namespace BookHive.Services;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/")
            || context.Request.Path.StartsWithSegments("/auth")
            || context.Request.Path.StartsWithSegments("/catalog")
            || context.Request.Path.Value!.Contains('.'))
        {
            await next(context);
            return;
        }

        if (!context.Session.IsUserAuthenticated())
        {
            context.Session.SetString("LinkToVisit", context.Request.Path);
            context.Response.Redirect("/Auth/Login/", true);
            return;
        }

        await next(context);
    }
}