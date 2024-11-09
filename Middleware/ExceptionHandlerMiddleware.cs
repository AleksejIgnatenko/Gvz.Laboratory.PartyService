using Gvz.Laboratory.PartyService.Exceptions;
using Serilog;
using System.Text.Json;

namespace Gvz.Laboratory.PartyService.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (PartyValidationException ex)
            {
                var statusCode = StatusCodes.Status400BadRequest;

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { error = ex.Errors });
                await context.Response.WriteAsync(result);
            }
            catch (RepositoryException ex)
            {
                var statusCode = StatusCodes.Status409Conflict;

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }
}
