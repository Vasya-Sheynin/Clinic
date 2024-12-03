using Application.Dto;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace Infrastructure
{
    public interface IAuthenticationManager
    {
        Task<(string AccessToken, string RefreshToken)?> HandleLoginAsync(LoginDto loginDto);
        Task<(string AccessToken, string RefreshToken)?> HandleRegisterAsync(RegisterDto registerDto);
        Task<(string AccessToken, string RefreshToken)?> HandleRefreshAsync(RefreshDto refreshDto);
        Task HandleLogoutAsync(string userName);
    }
}
