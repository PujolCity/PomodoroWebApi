using Microsoft.AspNetCore.Builder;

namespace PomodoroWebApp.Infrastructure.Middlewares;

public static class ExceptionHandlingMiddlewareExtension
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}