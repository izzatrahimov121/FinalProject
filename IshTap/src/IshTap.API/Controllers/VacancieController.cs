using IshTap.Business.DTOs.Vacancie;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class VacancieController : Controller
{
    private readonly IVacancieService _vacancieService;
    public VacancieController(IVacancieService vacancieService)
    {
        _vacancieService = vacancieService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var vacancies = await _vacancieService.FindAllAsync();
            return Ok(vacancies);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var course = await _vacancieService.FindByIdAsync(id);
            return Ok(course);
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


    [HttpGet("searchByTitle/{title}")]
    public async Task<IActionResult> GetByName(string title)
    {
        try
        {
            var result = await _vacancieService.FindByConditionAsync(n => n.Title != null ? n.Title.Contains(title) : true);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, VacancieUpdateDto vacancie)
    {
        try
        {
            await _vacancieService.UpdateAsync(id, vacancie);
            return StatusCode((int)HttpStatusCode.OK);

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



    [HttpPost("")]
    public async Task<IActionResult> Post(VacancieCreateDto vacancie)
    {
        try
        {
            await _vacancieService.CreateAsync(vacancie);
            return StatusCode((int)HttpStatusCode.Created);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _vacancieService.Delete(id);
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


    [HttpGet("FilterByCategoryandJobtype/{categoryId},{jobtypeId}")]
    public async Task<IActionResult> FilterByCategoryAndJobTypeAsync(int categoryId, int jobtypeId)
    {
        try
        {
            return Ok(await _vacancieService.FilterByCategoryAndJobTypeAsync(categoryId, jobtypeId));
        }
        catch (ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpGet("FilterByCategory/{categoryId}")]
    public async Task<IActionResult> FilterByCategoryAsync(int categoryId)
    {
        try
        {
            return Ok(await _vacancieService.FilterByCategoryAsync(categoryId));
        }
        catch (ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("FiterByDate/{date}")]
    public async Task<IActionResult> FiterByDate(int date)
    {
        try
        {
            return Ok(await _vacancieService.FiterByDateAsync(date)) ;
        }
        catch (Exception ex)
        {

            throw;
        }
    }


}
