using System.Net;

namespace MeuProjeto.Core.Exceptions
{
    public class MyUnauthorizedException : BaseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public MyUnauthorizedException(string message = "Não autorizado.") : base(message) { }
    }
}
