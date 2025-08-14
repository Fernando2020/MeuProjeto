using Serilog;

namespace MeuProjeto.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Log.Information("Requisição {Method} {Path}", context.Request.Method, context.Request.Path);
            await _next(context);
            Log.Information("Resposta {StatusCode} for {Path}", context.Response.StatusCode, context.Request.Path);
        }
    }
}
