using FluentValidation;
using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Application.Extensions;
using MeuProjeto.Core.Data;
using MeuProjeto.Core.Exceptions;
using MeuProjeto.Core.Repositories;
using MeuProjeto.Core.Security;

namespace MeuProjeto.Application.UseCases.Users.RefreshToken
{
    public class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _repo;
        private readonly IValidator<RefreshTokenRequestDto> _validator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ITokenGenerator _tokenGenerator;

        public RefreshTokenUseCase(IUnitOfWork uow, IUserRepository repo, IValidator<RefreshTokenRequestDto> validator, IRefreshTokenGenerator refreshTokenGenerator, ITokenGenerator tokenGenerator)
        {
            _uow = uow;
            _repo = repo;
            _validator = validator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponseDto> ExecuteAsync(RefreshTokenRequestDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new MyValidationException(validationResult.Errors.ToStringList());

            var user = await _repo.GetByRefreshTokenAsync(request.RefreshToken);
            if (user == null || !_refreshTokenGenerator.Verify(user.RefreshTokenExpiryTime))
                throw new MyUnauthorizedException();

            user.RefreshToken = _refreshTokenGenerator.GenerateToken();
            user.RefreshTokenExpiryTime = _refreshTokenGenerator.GetExpirationDate();

            await _repo.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = _tokenGenerator.GenerateToken(user),
                RefreshToken = user.RefreshToken
            };
        }
    }
}
