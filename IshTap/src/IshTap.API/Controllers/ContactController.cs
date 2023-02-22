using IshTap.Business.DTOs.GetInTouch;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGetInTouchService _getInTouchService;

        public ContactController(UserManager<AppUser> userManager, IGetInTouchService getInTouchService)
        {
            _userManager = userManager;
            _getInTouchService = getInTouchService;
        }

        [Authorize]
        [HttpPost("created")]
        public async Task<IActionResult> Created(GetInTouchDto getInTouchDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if (user == null) { throw new NotFoundException("User not found"); }
                await _getInTouchService.CreateAsync(user.Id, getInTouchDto);
                return Ok("message sent successfully");
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


        [Authorize(Roles = "Admin,Member")]
        [HttpGet("messages")]
        public async Task<IActionResult> Messages()
        {
            try
            {
                var messages = await _getInTouchService.Messages();
                return Ok(messages);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _getInTouchService.Delete(id);
                return Ok("Deleted");
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

    }
}
