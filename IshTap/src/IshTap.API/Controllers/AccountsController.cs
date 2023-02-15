using IshTap.Business.DTOs.Auth;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.Core.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly SignInManager<AppUser> _signInManager;
    public AccountsController(IAuthService authService, SignInManager<AppUser> signInManager)
    {
        _authService = authService;
        _signInManager = signInManager;
    }


    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
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

    [HttpPost("[action]")]
    public async Task<IActionResult> Verification ([FromForm] int code)
    {
        try
        {
            await _authService.VerificationAsync(code);
            return Ok("User successfully created");
        }
        catch(ArgumentNullException ex)
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

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
    {
        try
        {
            var tokenResponse = await _authService.LoginAsync(loginDto);
            return Ok(tokenResponse);
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

    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Ok("User successfully logout");
        }
        catch (LogoutFailException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }


    [HttpPost("ForgotPassword/{email}")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            await _authService.ForgotPasswordAsync(email);
            return Ok("Kod gonderildi. Zehmet olmasa mailinize baxin");
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
