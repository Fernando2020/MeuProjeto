namespace MeuProjeto.Application.DTOs.Exceptions
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public List<string>? Errors { get; set; }

        public static ErrorResponse From(Exception ex, int statusCode, List<string>? errors = null) =>
            new() { StatusCode = statusCode, Errors = errors };
    }
}
