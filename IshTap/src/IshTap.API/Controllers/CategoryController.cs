using IshTap.Business.DTOs.Category;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Member")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet("All")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = await _categoryService.FindAllAsync();
                return Ok(categories);
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
                var category = await _categoryService.FindByIdAsync(id);
                if (category is null)
                {
                    throw new NotFoundException("Category not found");
                }
                return Ok(category);
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

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(int id, CategoryUpdateDto category)
        {
            try
            {
                await _categoryService.UpdateAsync(id, category);
                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (ArgumentNullException ex)
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

        [HttpPost("created")]
        public async Task<IActionResult> Post(CategoryCreateDto category)
        {
            try
            {
                await _categoryService.CreateAsync(category);
                return StatusCode((int)HttpStatusCode.Created);
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

        [HttpDelete("deleted/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.Delete(id);
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

        [HttpPost("TopCategory")]
        public async Task<IActionResult> TopCategory(int count)
        {
            try
            {
                return Ok(await _categoryService.TopCategory(count));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
