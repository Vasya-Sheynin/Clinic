using Microsoft.AspNetCore.Http;

namespace Infrastructure.SessionStorageService;

public interface ISessionStorageService
{
    void SetAccessTokenCookie(HttpResponse response, string token);

    void SetRefreshTokenCookie(HttpResponse response, string token);

    void DeleteCookie(HttpResponse response, string name);

    string? GetAccessToken(HttpRequest request);

    string? GetRefreshToken(HttpRequest request);
}
