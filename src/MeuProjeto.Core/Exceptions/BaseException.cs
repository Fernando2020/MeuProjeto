using System.Net;

namespace MeuProjeto.Core.Exceptions
{
    public abstract class BaseException : Exception
    {
        public abstract HttpStatusCode StatusCode { get; }

        protected BaseException(string message) : base(message) { }
    }
}
