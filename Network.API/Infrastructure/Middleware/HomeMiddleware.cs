using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Infrastructure.Middleware
{
    public class HomeMiddleware
    {
        private readonly RequestDelegate _next;

        public HomeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            
            var path = httpContext.Request.Path;
            if (path.HasValue && path.Value.Contains("/"))
            {
                httpContext.Response.Redirect("/Home/");
            }
            await _next(httpContext);
        }
    }
   
    public static class HomeMiddlewareExtensions
    {
        public static IApplicationBuilder UseHomeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HomeMiddleware>();
        }
    }

}
