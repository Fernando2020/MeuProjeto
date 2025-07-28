using MeuProjeto.Application.DTOs.Exceptions;
using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Application.UseCases.Users.Login;
using MeuProjeto.Application.UseCases.Users.RefreshToken;
using MeuProjeto.Application.UseCases.Users.Register;
using MeuProjeto.Application.UseCases.Users.UserProfile;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserProfileUseCase _userProfileUseCase;

        public UserController(ILoginUseCase loginUseCase, IRefreshTokenUseCase refreshTokenUseCase, IRegisterUseCase registerUseCase, IUserProfileUseCase userProfileUseCase)
        {
            _loginUseCase = loginUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
            _registerUseCase = registerUseCase;
            _userProfileUseCase = userProfileUseCase;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _loginUseCase.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _refreshTokenUseCase.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _registerUseCase.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _userProfileUseCase.ExecuteAsync();
            return Ok(result);
        }
    }
}
