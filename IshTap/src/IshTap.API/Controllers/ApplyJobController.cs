using IshTap.Business.DTOs.ApplyJob;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ApplyJobController : ControllerBase
{
	private readonly IApplyJobService _applyJobService;

    public ApplyJobController(IApplyJobService applyJobService)
    {
        _applyJobService = applyJobService;
    }

    [HttpPost("applyjob/{id}")]
    public async Task<IActionResult> ApplyJob(int id,[FromForm]ApplyJobDto applyJobDto)
    {
		try
		{
			await _applyJobService.Created(id,applyJobDto);
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
