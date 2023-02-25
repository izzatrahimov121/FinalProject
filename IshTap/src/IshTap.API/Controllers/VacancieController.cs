﻿using IshTap.Business.DTOs.Vacancie;
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
public class VacancieController : Controller
{
    private readonly IVacancieService _vacancieService;
    private readonly UserManager<AppUser> _userManager;
    public VacancieController(IVacancieService vacancieService, UserManager<AppUser> userManager)
    {
        _vacancieService = vacancieService;
        _userManager = userManager;
    }

    [HttpGet("All")]
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


    [HttpGet("GetById")]
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


    [HttpGet("SearchByTitle")]
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


    [HttpPut("Update")]
    public async Task<IActionResult> Put(int id,[FromForm]VacancieUpdateDto vacancie)
    {
        try
        {
            await _vacancieService.UpdateAsync(id, vacancie);
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
    public async Task<IActionResult> Post([FromForm]VacancieCreateDto vacancie)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _vacancieService.CreateAsync(user.Id,vacancie);
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


    [HttpDelete("Deleted")]
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


    [HttpGet("FilterByCategoryandJobtype")]
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


    [HttpGet("FilterByCategory")]
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

    [HttpGet("FiterByDate")]
    public async Task<IActionResult> FiterByDateAsync(int date)
    {
        try
        {
            return Ok(await _vacancieService.FiterByDateAsync(date));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpGet("LastVacancies")]
    public async Task<IActionResult> LastVacanciesAsync()
    {
        var last = await _vacancieService.LastVacanciesAsync();
        return Ok(last);
    }

    [HttpGet("FilterByCondition")]
    public async Task<IActionResult> FilterByConditionAsync(int categoryId, int jobtypeId, int minSalary, int maxSalary)
    {
        try
        {
            var result = await _vacancieService.FilterByConditionAsync(categoryId, jobtypeId, minSalary, maxSalary);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

}
