using IshTap.Business.DTOs.Auth;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserProfileController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserProfileService _userProfileService;

    public UserProfileController(UserManager<AppUser> userManager,
                                 IUserProfileService userProfileService)
    {
        _userManager = userManager;
        _userProfileService = userProfileService;
    }

    [HttpGet("Profile")]
    public async Task<IActionResult> GetUserInfoFromLogined()
    {
        try
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            if (user is null) { throw new NotFoundException("User not found"); }
            UserInfoDto userInfo = new()
            {
                UserName = user.UserName,
                Fullname = user.Fullname,
                Email = user.Email,
                Image = user.Image,
            };
            return Ok(userInfo);
        }
        catch (NotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.NotFound);
        }
    }

    [HttpPost("profilphoto")]
    public async Task<IActionResult> ChenceImage([FromForm] UserImageDto Image)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user is null) { throw new NotFoundException("User not found"); }
            await _userProfileService.ChenceImageAsync(user.Id, Image);
            return Ok("Image changed successfully");
        }
        catch (NotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("vacancies")]
    public async Task<IActionResult> Vacancies()
    {
        try
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user is null) { throw new NotFoundException("User not found"); }
            var resultVacancies = await _userProfileService.UserVacanciesAsync(user.Id);
            return Ok(resultVacancies);
        }
        catch (NotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("cvs")]
    public async Task<IActionResult> CVs()
    {
        try
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            if (user is null) { throw new NotFoundException("User not found"); }
            var resultCV = await _userProfileService.UserCVsAsync(user.Id);
            return Ok(resultCV);
        }
        catch(NotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
