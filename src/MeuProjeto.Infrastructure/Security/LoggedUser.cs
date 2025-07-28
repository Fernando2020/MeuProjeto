using MeuProjeto.Core.Entities;
using MeuProjeto.Core.Exceptions;
using MeuProjeto.Core.Security;
using MeuProjeto.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MeuProjeto.Infrastructure.Security
{
    public class LoggedUser : ILoggedUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public LoggedUser(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public Guid GetUserId()
        {
            var sub = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(sub) || !Guid.TryParse(sub, out var userId))
                throw new MyUnauthorizedException();

            return userId;
        }

        public async Task<User> GetUserAsync()
        {
            var userId = GetUserId();
            
            return await _context.Users
                    .AsNoTracking()
                    .Where(u => u.Id == userId) 
                    .SingleOrDefaultAsync()
                    ?? throw new MyUnauthorizedException("Usuário não encontrado.");
        }
    }
}
