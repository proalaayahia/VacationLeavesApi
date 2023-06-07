using System.Net;

namespace minimalApiDemo.Middlewares;

public class GlobalHandleExceptionCustomMiddleware : IMiddleware
{
    private readonly ILogger<GlobalHandleExceptionCustomMiddleware> _logger;

    public GlobalHandleExceptionCustomMiddleware(ILogger<GlobalHandleExceptionCustomMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(Exception ex)
        {
            _logger.LogError("something went wrong! ", ex);
            await HandleExceptionsAsync(context, ex);
        }
    }
    private async Task HandleExceptionsAsync(HttpContext context,Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new
        {
            StatusCode=context.Response.StatusCode,
            Message=$"internal server error from custom middleware, {exception.Message}"
        }.ToString()!);
    }
}
public static class AddDependencies
{
    public static IServiceCollection GlobalExceptionsCustomMiddleware(this IServiceCollection services)
    {
        services.AddTransient<GlobalHandleExceptionCustomMiddleware>();
        return services;
    }
    public static void AddGlobalExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<GlobalHandleExceptionCustomMiddleware>();
    }
}

