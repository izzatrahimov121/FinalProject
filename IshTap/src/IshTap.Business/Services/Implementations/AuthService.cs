using IshTap.Business.DTOs.Auth;
using IshTap.Business.Exceptions;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace IshTap.Business.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMailService _mailService;
    private readonly ITokenHandler _tokenHandler;

    public AuthService(UserManager<AppUser> userManager
                     , IMailService mailService
                     , ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _mailService = mailService;
        _tokenHandler = tokenHandler;
    }


    public async Task RegisterAsync(RegisterDto registerDto)
    {
        AppUser user = new()
        {
            Email = registerDto.Email,
            UserName = registerDto.Username,
            Fullname = registerDto.Fullname,
        };

        var identityResult = await _userManager.CreateAsync(user, registerDto.Password);
        if (!identityResult.Succeeded)
        {
            string errors = String.Empty;
            int count = 0;
            foreach (var error in identityResult.Errors)
            {
                errors += count != 0 ? $",{error.Description}" : $"{error.Description}";
                count++;
            }

            throw new UserCreateFailException(errors);
        }

        var result = await _userManager.AddToRoleAsync(user, Roles.BasicUser.ToString());
        if (!result.Succeeded)
        {
            string errors = String.Empty;
            int count = 0;
            foreach (var error in result.Errors)
            {
                errors += count != 0 ? $",{error.Description}" : $"{error.Description}";
                count++;
            }
            throw new RoleCreateFailException(errors);
        }
    }

    public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UsernameOrEmail);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(loginDto.UsernameOrEmail);
        }
        if (user is null) throw new AuthFailException("Username or password incorrect!");

        var check = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!check) throw new AuthFailException("Username or password incorrect!");

        //Create Jwt
        var tokenResponse = await _tokenHandler.GenerateTokenAsync(user, 1);
        return tokenResponse;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        //throw new LogoutFailException("user is not logged in");
    }

    public async Task ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new AuthFailException("not user");
        }

        Random rnd = new Random();
        var code = rnd.Next(100000, 999999);
        MailRequestDto mailRequest = new()
        {
            ToEmail = user.Email,
            Subject = "Reset passvord",
            Body = $"<h3>Doğrulama kodunuz:{code} </h3>" +
            $"<br>Kodu heçkimlə paylaşmayın <br> " +
            $"Copyright ©2023 İş Tap | All rights reserved.",
        };
        await _mailService.SendEmailAsync(mailRequest);
    }
}
