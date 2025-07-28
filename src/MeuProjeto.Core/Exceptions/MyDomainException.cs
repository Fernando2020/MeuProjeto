using System.Net;

namespace MeuProjeto.Core.Exceptions
{
    public class MyDomainException : BaseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public MyDomainException(string message) : base(message) { }
    }
}
