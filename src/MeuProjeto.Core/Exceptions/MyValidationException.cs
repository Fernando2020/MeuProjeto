using System.Net;

namespace MeuProjeto.Core.Exceptions
{
    public class MyValidationException : BaseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public List<string> Errors { get; }

        public MyValidationException(List<string> errors)
            : base("Não validado.")
        {
            Errors = errors ?? new List<string>();
        }
    }
}
