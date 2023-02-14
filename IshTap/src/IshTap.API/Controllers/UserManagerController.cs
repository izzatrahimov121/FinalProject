using IshTap.Business.DTOs.Auth;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class UserManagerController : ControllerBase
    {
        private readonly IUserManagerService _userManagerService;

        public UserManagerController(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _userManagerService.FindAllAsync());
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _userManagerService.FindByIdAsync(id);
                return Ok(user);
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreatedUser([FromForm]RegisterDto registerDto, [FromForm]Roles role)
        {
            try
            {
                await _userManagerService.CreateAsync(registerDto, role);
                return Ok("User successfully created");
            }
            catch (UserCreateFailException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RoleCreateFailException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateUserRole([FromForm]string id, [FromForm]Roles role)
        {
            try
            {
                await _userManagerService.UpdateUserRoleAsync(id, role);
                return Ok("Role successfully updated");
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RoleCreateFailException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddUserRole([FromForm] string id, [FromForm]Roles role)
        {
            try
            {
                await _userManagerService.AddUserRoleAsync(id, role);
                return Ok("Role successfully added");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RoleCreateFailException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveUserRoleAsync([FromForm] string id, [FromForm] Roles role)
        {
            try
            {
                await _userManagerService.RemoveUserRoleAsync(id, role);
                return Ok("Role successfully remove");
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleted([FromForm]string id)
        {
            try
            {
                await _userManagerService.DeleteUser(id);
                return Ok("User successfully deleted");
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
}
