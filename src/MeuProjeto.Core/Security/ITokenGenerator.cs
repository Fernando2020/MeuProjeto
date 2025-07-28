using MeuProjeto.Core.Entities;

namespace MeuProjeto.Core.Security
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}
