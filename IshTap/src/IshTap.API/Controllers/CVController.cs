using IshTap.Business.DTOs.CV;
using IshTap.Business.DTOs.Vacancie;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Implementations;
using IshTap.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CVController : ControllerBase
    {
        private readonly ICVService _cvService;

        public CVController(ICVService cvService)
        {
            _cvService = cvService;
        }


        [HttpGet("")]
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


        [HttpGet("{id}")]
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

        [HttpPut("{id}")]
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
        public async Task<IActionResult> Post([FromForm] CVCreatedDto cv)
        {
            try
            {
                await _cvService.CreateAsync(cv);
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

        [HttpDelete("{id}")]
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
}
