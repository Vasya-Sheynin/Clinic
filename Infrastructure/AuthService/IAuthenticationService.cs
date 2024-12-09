using Application.Dto;

namespace Infrastructure.AuthService;

public interface IAuthenticationService
{
    Task<TokensModel> HandleLoginAsync(LoginDto loginDto);
    Task<TokensModel> HandleRegisterAsync(RegisterDto registerDto);
    Task<TokensModel> HandleRefreshAsync(RefreshDto refreshDto);
    Task HandleLogoutAsync(LogoutDto logoutDto);
}
