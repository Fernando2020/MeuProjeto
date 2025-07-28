using MeuProjeto.Core.Entities;

namespace MeuProjeto.Core.Security
{
    public interface ILoggedUser
    {
        Guid GetUserId();
        Task<User> GetUserAsync();
    }

}
