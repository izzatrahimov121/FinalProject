﻿using IshTap.Business.DTOs.Auth;

namespace IshTap.Business.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto registerDto);
    Task<TokenResponseDto> LoginAsync(LoginDto loginDto);
    Task LogoutAsync();
    Task ForgotPasswordAsync(string email);
}
