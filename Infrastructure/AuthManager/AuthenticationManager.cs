using Users;
using Application.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Infrastructure.Exceptions;
using Application.Validation;
using FluentValidation;

namespace Infrastructure.AuthManager
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly double _refreshTokenLifetime;
        private readonly double _accessTokenLifetime;

        public AuthenticationManager(UserManager<User> accountManager, IConfiguration configuration)
        {
            _userManager = accountManager;
            _configuration = configuration;

            _refreshTokenLifetime = Convert.ToDouble(
                _configuration.GetSection("RefreshSettings").GetSection("expires").Value);
            _accessTokenLifetime = Convert.ToDouble(
                _configuration.GetSection("JwtSettings").GetSection("expires").Value);
        }

        public async Task<(string AccessToken, string RefreshToken)> HandleLoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new LoginException();
            }

            var accessToken = await CreateAccessToken(user);
            var refreshToken = CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_refreshTokenLifetime);
            await _userManager.UpdateAsync(user);

            return (AccessToken: accessToken, RefreshToken: refreshToken);
        }

        public async Task<(string AccessToken, string RefreshToken)> HandleRegisterAsync(RegisterDto registerDto)
        {
            var validator = new RegisterDtoValidator();
            await validator.ValidateAndThrowAsync(registerDto);

            var user = new User
            {
                CreatedAt = DateTime.UtcNow,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                StringBuilder exceptionMessage = new StringBuilder(string.Empty);
                foreach (var error in result.Errors)
                {
                    exceptionMessage.AppendLine($"{error.Code}, {error.Description}");
                }
                throw new RegisterException(exceptionMessage.ToString());
            }


            await _userManager.AddToRoleAsync(user, "Patient");

            var accessToken = await CreateAccessToken(user);
            var refreshToken = CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_refreshTokenLifetime);

            await _userManager.UpdateAsync(user);

            return (AccessToken: accessToken, RefreshToken: refreshToken);
        }

        public async Task<(string AccessToken, string RefreshToken)> HandleRefreshAsync(RefreshDto refreshDto)
        {
            var accessToken = refreshDto.AccessToken;
            var refreshToken = refreshDto.RefreshToken;

            if (accessToken == null || refreshToken == null)
            {
                throw new InvalidTokenException();
            }

            var principal = GetClaimsPrincipal(accessToken);

            var userName = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new InvalidTokenException();
            }

            var newAccessToken = await CreateAccessToken(user);
            var newRefreshToken = CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_refreshTokenLifetime);

            await _userManager.UpdateAsync(user);

            return (AccessToken: newAccessToken, RefreshToken: newRefreshToken);
        }

        public async Task HandleLogoutAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiry = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
                await _userManager.UpdateAsync(user);
            }
        }

        private async Task<string> CreateAccessToken(User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private string CreateRefreshToken()
        {
            var token = new byte[64];

            var generator = RandomNumberGenerator.Create();
            generator.GetBytes(token);

            return Convert.ToBase64String(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings.GetSection("validIssuer").Value,
                audience: jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_accessTokenLifetime),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }

        private ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("JWT_KEY");

            var validation = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                ValidAudience = jwtSettings.GetSection("validAudience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }
    }
}
