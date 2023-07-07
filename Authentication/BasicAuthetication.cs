using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIDemo2.Authentication
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BasicAuthetication
    {
        private readonly RequestDelegate _next;

        public BasicAuthetication(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();

            if (authorizationHeader != null && authorizationHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                var token = authorizationHeader.Substring("Basic ".Length).Trim();
                var credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var credentials = credentialsAsEncodedString.Split(':');

                string userName=credentials[0];
                string password=credentials[1];

                // _service.getUserValidate(userName,password);

                if (userName == "virat" && password=="virat@123")
                   await _next.Invoke(httpContext);
                else
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Unauthorized");
                    return;
                }

            }
            else
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Unauthorized");
                return;
            }
           
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BasicAutheticationExtensions
    {
        public static IApplicationBuilder UseBasicAuthetication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthetication>();
        }
    }
}
