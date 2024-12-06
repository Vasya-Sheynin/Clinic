using Infrastructure.AuthService.TokenOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.SessionStorageService;

public class SessionStorageService : ISessionStorageService
{
    private readonly IOptions<RefreshTokenOptions> _refreshTokenOptions;

    public SessionStorageService(IOptions<RefreshTokenOptions> refreshTokenOptions)
    {
        _refreshTokenOptions = refreshTokenOptions;
    }

    public void DeleteCookie(HttpResponse response, string name)
    {
        response.Cookies.Delete(name);
    }

    public string? GetAccessToken(HttpRequest request)
    {
        var token = request.Cookies["AccessToken"];

        return token;
    }

    public string? GetRefreshToken(HttpRequest request)
    {
        var token = request.Cookies["RefreshToken"];

        return token;
    }

    public void SetAccessTokenCookie(HttpResponse response, string token)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(_refreshTokenOptions.Value.ExpirationTime),
            Secure = true,
            HttpOnly = true,
        };

        response.Cookies.Append("AccessToken", token, cookieOptions);
    }

    public void SetRefreshTokenCookie(HttpResponse response, string token)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(_refreshTokenOptions.Value.ExpirationTime),
            Secure = true,
            HttpOnly = true,
        };

        response.Cookies.Append("RefreshToken", token, cookieOptions);
    }
}
