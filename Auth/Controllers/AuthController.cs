using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Dto;
using Infrastructure.AuthManager;
using Infrastructure.SessionStorageManager;

namespace Auth.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationManager _authManager;
        private readonly ISessionStorageManager _sessionStorageManager;

        public AuthController(IAuthenticationManager authManager, ISessionStorageManager sessionStorageManager)
        {
            _authManager = authManager;
            _sessionStorageManager = sessionStorageManager;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var tokens = await _authManager.HandleRegisterAsync(registerDto);

            _sessionStorageManager.SetAccessTokenCookie(Response, tokens.AccessToken);
            _sessionStorageManager.SetRefreshTokenCookie(Response, tokens.RefreshToken);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var tokens = await _authManager.HandleLoginAsync(loginDto);    
            
            _sessionStorageManager.SetAccessTokenCookie(Response, tokens.AccessToken);
            _sessionStorageManager.SetRefreshTokenCookie(Response, tokens.RefreshToken);

            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var userName = HttpContext.User.Identity?.Name;
            await _authManager.HandleLogoutAsync(userName);
            
            _sessionStorageManager.DeleteCookie(Response, "AccessToken");
            _sessionStorageManager.DeleteCookie(Response, "RefreshToken");

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh()
        {
            var accessToken = _sessionStorageManager.GetAccessToken(Request);
            var refreshToken = _sessionStorageManager.GetRefreshToken(Request);
            var newTokens = await _authManager.HandleRefreshAsync(new RefreshDto(accessToken, refreshToken));

            _sessionStorageManager.SetAccessTokenCookie(Response, newTokens.AccessToken);
            _sessionStorageManager.SetRefreshTokenCookie(Response, newTokens.RefreshToken);

            return Ok();
        }
    }
}
