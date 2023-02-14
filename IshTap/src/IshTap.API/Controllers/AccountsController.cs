using IshTap.Business.DTOs.Auth;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AccountsController(IAuthService authService, RoleManager<IdentityRole> roleManager)
    {
        _authService = authService;
        _roleManager = roleManager;
    }


    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        try
        {
            await _authService.RegisterAsync(registerDto/*, VerificationCode*/);
            //return RedirectToAction(nameof(Verification));
            return Ok("User successfully created");
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
    public async Task<IActionResult> Login(LoginDto loginDto)
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
            await _authService.LogoutAsync();
            return Ok("User successfully logout");
        }
        catch(LogoutFailException ex)
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

    [HttpGet("Createroles")]
    public async Task<IActionResult> CreateRoles()
    {
        foreach (var role in Enum.GetValues(typeof(Roles)))
        {
            if (!await _roleManager.RoleExistsAsync(role.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }
        }

        return Ok("Roles Created");
    }
}
