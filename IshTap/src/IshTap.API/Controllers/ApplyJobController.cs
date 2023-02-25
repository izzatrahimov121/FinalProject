using IshTap.Business.DTOs.ApplyJob;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ApplyJobController : ControllerBase
{
	private readonly IApplyJobService _applyJobService;
    private readonly UserManager<AppUser> _userManager;

    public ApplyJobController(IApplyJobService applyJobService, UserManager<AppUser> userManager)
    {
        _applyJobService = applyJobService;
        _userManager = userManager;
    }

    [HttpPost("applyjob/{id}")]
    public async Task<IActionResult> ApplyJob(int id,[FromForm]ApplyJobCreateDto applyJobDto)
    {
		try
		{
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
			await _applyJobService.Created(id,user.Id, applyJobDto);
			return Ok();
		}
		catch(NotFoundException ex)
		{
			return BadRequest(ex.Message);
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
}
