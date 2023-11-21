using System.Net;

namespace RetroFootballAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }   

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException)
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";
                var errorResponse = new { error = "Resource not found" };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var errorResponse = new { error = "Internal server error" };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
