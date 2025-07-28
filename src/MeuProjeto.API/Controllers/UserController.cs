using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Application.UseCases.Users.Login;
using MeuProjeto.Application.UseCases.Users.RefreshToken;
using MeuProjeto.Application.UseCases.Users.Register;
using Microsoft.AspNetCore.Mvc;

namespace MeuProjeto.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILoginUseCase _loginUseCase;
        private readonly IRefreshTokenUseCase _refreshTokenUseCase;
        private readonly IRegisterUseCase _registerUseCase;

        public UserController(ILoginUseCase loginUseCase, IRefreshTokenUseCase refreshTokenUseCase, IRegisterUseCase registerUseCase)
        {
            _loginUseCase = loginUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
            _registerUseCase = registerUseCase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _loginUseCase.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _refreshTokenUseCase.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _registerUseCase.ExecuteAsync(request);
            return Ok(result);
        }
    }
}
