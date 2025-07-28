using MeuProjeto.Core.Security;
using System.Security.Cryptography;

namespace MeuProjeto.Infrastructure.Security
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly TimeSpan _expiration = TimeSpan.FromDays(7);

        public string GenerateToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public DateTime GetExpirationDate()
        {
            return DateTime.UtcNow.Add(_expiration);
        }

        public bool Verify(DateTime? refreshTokenExpiryTime)
        {
            return refreshTokenExpiryTime > DateTime.UtcNow;
        }
    }
}
