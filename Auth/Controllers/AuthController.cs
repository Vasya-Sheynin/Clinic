using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Dto;
using Infrastructure.AuthService;
using Infrastructure.SessionStorageService;

namespace Auth.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly ISessionStorageService _sessionStorageService;

    public AuthController(IAuthenticationService authManager, ISessionStorageService sessionStorageManager)
    {
        _authService = authManager;
        _sessionStorageService = sessionStorageManager;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var tokens = await _authService.HandleRegisterAsync(registerDto);

        _sessionStorageService.SetAccessTokenCookie(Response, tokens.AccessToken);
        _sessionStorageService.SetRefreshTokenCookie(Response, tokens.RefreshToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        var tokens = await _authService.HandleLoginAsync(loginDto);    
        
        _sessionStorageService.SetAccessTokenCookie(Response, tokens.AccessToken);
        _sessionStorageService.SetRefreshTokenCookie(Response, tokens.RefreshToken);

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        var userName = HttpContext.User.Identity?.Name;
        await _authService.HandleLogoutAsync(userName);
        
        _sessionStorageService.DeleteCookie(Response, "AccessToken");
        _sessionStorageService.DeleteCookie(Response, "RefreshToken");

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh()
    {
        var accessToken = _sessionStorageService.GetAccessToken(Request);
        var refreshToken = _sessionStorageService.GetRefreshToken(Request);
        var newTokens = await _authService.HandleRefreshAsync(new RefreshDto(accessToken, refreshToken));

        _sessionStorageService.SetAccessTokenCookie(Response, newTokens.AccessToken);
        _sessionStorageService.SetRefreshTokenCookie(Response, newTokens.RefreshToken);

        return Ok();
    }
}
