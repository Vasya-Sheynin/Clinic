using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Application.Dto;

namespace Auth.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationManager _authManager;
        public AuthController(IAuthenticationManager authManager)
        {
            _authManager = authManager;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var tokens = await _authManager.HandleRegisterAsync(registerDto);

            SetAccessTokenCookie(tokens.Value.AccessToken);
            SetRefreshTokenCookie(tokens.Value.RefreshToken);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var tokens = await _authManager.HandleLoginAsync(loginDto);
        
            if (tokens != null)
            {
                SetAccessTokenCookie(tokens.Value.AccessToken);
                SetRefreshTokenCookie(tokens.Value.RefreshToken);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var userName = HttpContext.User.Identity?.Name;
            await _authManager.HandleLogoutAsync(userName);

            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody]RefreshDto refreshDto)
        {
            var tokens = await _authManager.HandleRefreshAsync(refreshDto);

            SetAccessTokenCookie(tokens.Value.AccessToken);
            SetRefreshTokenCookie(tokens.Value.RefreshToken);

            return Ok();
        }

        private void SetAccessTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1),
                Secure = true,
                HttpOnly = true,
            };

            Response.Cookies.Append("AccessToken", token, cookieOptions);
        }

        private void SetRefreshTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                Secure = true,
                HttpOnly = true,
            };

            Response.Cookies.Append("RefreshToken", token, cookieOptions);
        }
    }
}
