using IshTap.Business.DTOs.CV;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CVController : ControllerBase
{
    private readonly ICVService _cvService;
    private readonly UserManager<AppUser> _userManager;

    public CVController(ICVService cvService, UserManager<AppUser> userManager)
    {
        _cvService = cvService;
        _userManager = userManager;
    }


    [HttpGet("AllCv")]
    public async Task<IActionResult> GetAll(int skipt, int take)
    {
        try
        {
            if (skipt == null || skipt<0) { skipt = 0; }
            if (take == null || take<0) { take = 1; }
            var cvs = await _cvService.FindAllAsync(skipt,take);
            return Ok(cvs);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var cv = await _cvService.FindByIdAsync(id);
            return Ok(cv);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [Authorize]
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Put(int id, [FromForm] CVUpdateDto cv)
    {
        try
        {
            await _cvService.UpdateAsync(id, cv);
            return StatusCode((int)HttpStatusCode.OK);
        }
        catch (IncorrectFileFormatException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (IncorrectFileSizeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

    }

    [Authorize]
    [HttpPost("Created")]
    public async Task<IActionResult> Post([FromForm] CVCreatedDto cv)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _cvService.CreateAsync(user.Id, cv);
            return StatusCode((int)HttpStatusCode.Created);
        }
        catch (IncorrectFileFormatException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (IncorrectFileSizeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [Authorize]
    [HttpDelete("Deleted/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _cvService.Delete(id);
            return Ok("Deleted");
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("LastCVs")]
    public async Task<IActionResult> LastCVs(int count)
    {
        try
        {
            var last = await _cvService.LastCVsAsync(count);
            return Ok(last);
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
