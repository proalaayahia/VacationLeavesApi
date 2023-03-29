using System.Net;
using VacationLeavesApi.Data;

namespace VacationLeavesApi.Middlewares
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("something went wrong ", ex);
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware. "+exception.Message
            }.ToString());
        }
    }
    public static class HandleExceptionsGlobally
    {
        public static IServiceCollection CustomMiddlewareRegistering(this IServiceCollection services)
        {
            services.AddTransient<ErrorHandlerMiddleware>();
            return services;
        }
        public static void ConfigureCustomeExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
