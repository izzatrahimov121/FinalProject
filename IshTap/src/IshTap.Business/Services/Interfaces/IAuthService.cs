using IshTap.Business.DTOs.Auth;

namespace IshTap.Business.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto registerDto);
    Task<TokenResponseDto> LoginAsync(LoginDto loginDto);
    Task VerificationAsync(int code);
    Task ForgotPasswordAsync(ForgotPasswordDto forgotPassword);
    Task ResetPasswordAsync(string email, string token, ResetPasswordDto resetPassword);
}
