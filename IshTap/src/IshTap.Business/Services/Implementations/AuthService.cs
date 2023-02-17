using IshTap.Business.DTOs.Auth;
using IshTap.Business.Exceptions;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IshTap.Business.Services.Implementations;

public class AuthService : IAuthService
{
    private static string? Fullname { get; set; }
    private static string? Username { get; set; }
    private static string? Email { get; set; }
    private static string? Password { get; set; }
    private static DateTime? CodeLifeTime { get; set; }
    private static int? Code { get; set; }


    private readonly UserManager<AppUser> _userManager;

    private readonly IMailService _mailService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenHandler _tokenHandler;

    public AuthService(UserManager<AppUser> userManager
                     , IMailService mailService
                     , ITokenHandler tokenHandler
                     , SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _mailService = mailService;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
    }

    private async Task SendMailResetToken(string email,string link)
    {
        MailRequestDto mailRequest = new()
        {
            ToEmail = email,
            Subject = "Account verification process",
            Body = $"<a href = \"{link}\"> {link} </a>" +
           $"<br>Kodu heçkimlə paylaşmayın <br> " +
           $"Copyright ©2023 İş Tap | All rights reserved.",
        };
        await _mailService.SendEmailAsync(mailRequest);
    }
    private async Task SendMailCode(string email)
    {

        Random rnd = new Random();
        var code = rnd.Next(100000, 999999);
        MailRequestDto mailRequest = new()
        {
            ToEmail = email,
            Subject = "Account verification process",
            Body = $"<h3>Doğrulama kodunuz: {code} </h3>" +
            $"<br>Kodu heçkimlə paylaşmayın <br> " +
            $"Copyright ©2023 İş Tap | All rights reserved.",
        };
        await _mailService.SendEmailAsync(mailRequest);
        Code = code;
        CodeLifeTime = DateTime.Now.AddSeconds(300);
    }
    private async Task CreatedUserAsync()
    {
        AppUser user = new()
        {
            Email = Email,
            UserName = Username,
            Fullname = Fullname,
        };
        var identityResult = await _userManager.CreateAsync(user, Password);
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
    public async Task RegisterAsync(RegisterDto registerDto)
    {
        if (registerDto is null)
        {
            throw new ArgumentNullException("Null value");
        }
        Fullname = registerDto.Fullname;
        Username = registerDto.Username;
        Email = registerDto.Email;
        Password = registerDto.Password;
        await SendMailCode(Email);
    }
    public async Task VerificationAsync(int code)
    {
        if (Fullname == null || Username == null || Email == null || Password == null || Code == null || code == null)
        {
            throw new ArgumentNullException();
        }
        if (DateTime.Now>=CodeLifeTime)
        {
            throw new NotFoundException("Time is over!");
        }
        if (Code == code)
        {
            await CreatedUserAsync();
        }
        else
        {
            throw new NotFoundException("Wrong code! Please try again");
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
        var tokenResponse = await _tokenHandler.GenerateTokenAsync(user, 10);

        return tokenResponse;
    }
    public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPassword, string link)
    {
        //var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
        //if (user == null) { throw new NotFoundException("User not found"); }
        await SendMailResetToken(forgotPassword.Email, link);
    }

    public async Task ResetPasswordAsync(ResetPasswordDto resetPassword)
    {
        var test = 1;
        test = 3;
    }
}
