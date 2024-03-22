using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ElectronicCommerce.MiddleWares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if(path.Value.StartsWith("/customer/customer/login") || path.Value.StartsWith("/customer/customer/register")
                || path.Value.StartsWith("/customer/customer/GoogleLogin") || path.Value.StartsWith("/customer/customer/googleresponse")
                || path.Value.StartsWith("/customer/customer/accountlogin") || path.Value.StartsWith("/customer/customer/loginsuccess")
                || path.Value.StartsWith("/customer/customer/forgot"))
            {
                return _next(httpContext);
            }
            else if (path.Value.StartsWith("/customer"))
            {
                Debug.WriteLine("Customer");
                if (httpContext.Session.GetString("customer") == null)
                {
                    httpContext.Response.Redirect("/customer/customer/login");
                }
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomerMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewareClassTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomerMiddleware>();
        }
    }
}
