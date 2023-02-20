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
[Authorize]
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
    public async Task<IActionResult> Get()
    {
        try
        {
            var cvs = await _cvService.FindAllAsync();
            return Ok(cvs);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
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
        catch (FormatException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Put(int id,CVUpdateDto cv)
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
        catch (BadRequestException ex)
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

    [HttpPost("Created")]
    public async Task<IActionResult> Post([FromForm]CVCreatedDto cv)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _cvService.CreateAsync(user.Id ,cv);
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
        catch (FormatException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("LastVacancies")]
    public async Task<IActionResult> LastVacancies()
    {
        var last = await _cvService.LastVacanciesAsync();
        return Ok(last);
    }
}
