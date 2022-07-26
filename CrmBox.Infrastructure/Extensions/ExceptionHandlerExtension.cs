using CrmBox.Infrastructure.MiddleLayers;
using Microsoft.AspNetCore.Builder;

namespace CrmBox.Infrastructure.Extensions;

public static class ExceptionHandlerExtension
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}