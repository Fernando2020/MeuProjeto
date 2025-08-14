using MeuProjeto.Application.DTOs.Exceptions;
using MeuProjeto.Core.Exceptions;
using Serilog;
using System.Net;
using System.Text.Json;

namespace MeuProjeto.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogException(ex, context);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static void LogException(Exception exception, HttpContext context)
        {
            Log.Error(exception, "Erro ao processar {Method} {Path}",
                context.Request.Method,
                context.Request.Path);
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            List<string>? errors = null;

            switch (exception)
            {
                case MyValidationException ve:
                    statusCode = (int)ve.StatusCode;
                    errors = ve.Errors;
                    break;

                case BaseException be:
                    statusCode = (int)be.StatusCode;
                    errors = [be.Message];
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errors = ["Ocorreu um erro inesperado."];
                    break;
            }

            context.Response.StatusCode = statusCode;
            var response = ErrorResponse.From(exception, statusCode, errors);

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}