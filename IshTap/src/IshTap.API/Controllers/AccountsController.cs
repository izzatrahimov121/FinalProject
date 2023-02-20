using IshTap.Business.DTOs.Auth;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace IshTap.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{

    private readonly IAuthService _authService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    public AccountsController(IAuthService authService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _authService = authService;
        _signInManager = signInManager;
        _userManager = userManager;
    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register( RegisterDto registerDto)
    {
        try
        {
            await _authService.RegisterAsync(registerDto);
            return Ok("Code sent. Please check your mailbox and complete the registration");
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("Verification")]
    public async Task<IActionResult> Verification(int code)
    {
        try
        {
            await _authService.VerificationAsync(code);
            return Ok("User successfully created");
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserCreateFailException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (RoleCreateFailException)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            var token = await _authService.LoginAsync(loginDto);
            //Token = token;
            return Ok(token);
        }
        catch (AuthFailException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("forgotpassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPassword)
    {
        try
        {
            //var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            //if (user == null) { throw new NotFoundException("User not found"); }
            //var token = _userManager.GeneratePasswordResetTokenAsync(user);
            //var link = Url.Action("ResetPassword", "Accounts", new { token, email = user.Email }, Request.Scheme);
            await _authService.ForgotPasswordAsync(forgotPassword);
            return Ok("Link sent. Please check your email");
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("resetpassword")]
    public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto resetPassword, string email, string token)
    {
        await _authService.ResetPasswordAsync(email,token,resetPassword);
        return Ok("Tamam");
    }
}
