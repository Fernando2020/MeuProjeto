using System.Net;

namespace MeuProjeto.Core.Exceptions
{
    public class MyNotFoundException : BaseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public MyNotFoundException(string message = "Não encontrado.") : base(message) { }
    }
}
