using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Dto;
using Infrastructure.AuthService;

namespace Auth.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthController(IAuthenticationService authManager)
    {
        _authService = authManager;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var tokens = await _authService.HandleRegisterAsync(registerDto);

        return Ok(tokens);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        var tokens = await _authService.HandleLoginAsync(loginDto);    

        return Ok(tokens);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        var userName = HttpContext.User.Identity?.Name;
        await _authService.HandleLogoutAsync(userName);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh([FromBody] RefreshDto tokensToRefresh)
    {
        var newTokens = await _authService.HandleRefreshAsync(tokensToRefresh);

        return Ok(newTokens);
    }
}
