using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.SessionStorageManager
{
    public class SessionStorageManager : ISessionStorageManager
    {
        private readonly double lifetime;

        public SessionStorageManager(IConfiguration configuration)
        {
            lifetime = Convert.ToDouble(
                configuration.GetSection("RefreshSettings").GetSection("expires").Value);
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
                Expires = DateTime.Now.AddDays(lifetime),
                Secure = true,
                HttpOnly = true,
            };

            response.Cookies.Append("AccessToken", token, cookieOptions);
        }

        public void SetRefreshTokenCookie(HttpResponse response, string token)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(lifetime),
                Secure = true,
                HttpOnly = true,
            };

            response.Cookies.Append("RefreshToken", token, cookieOptions);
        }
    }
}
