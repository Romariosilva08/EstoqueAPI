using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MinhaAPIEstoque.Extensions
{
    public static class Utf8EncodingMiddlewareExtensions
    {
        public static void UseEncoding(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers["Content-Type"] = "application/json; charset=utf-8";
                await next();
            });
        }
    }
}