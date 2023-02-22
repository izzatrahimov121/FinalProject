using IshTap.Business.DTOs.Auth;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;

namespace IshTap.API.Controllers
{
    [System.Web.Http.Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserManagerController : ControllerBase
    {
        private readonly IUserManagerService _userManagerService;
        private readonly UserManager<AppUser> _userManager;

        public UserManagerController(IUserManagerService userManagerService, UserManager<AppUser> userManager)
        {
            _userManagerService = userManagerService;
            _userManager = userManager;
        }

        [HttpGet("AllUser")]
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

        [HttpGet("GetByIdUser")]
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

        [HttpPost("CreatedUser")]
        public async Task<IActionResult> CreatedUser(RegisterDto registerDto,Roles role)
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

        [HttpPost("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(string id, Roles role)
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

        [HttpPost("AddUserRole")]
        public async Task<IActionResult> AddUserRole(string id, Roles role)
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

        [HttpPost("RemoveUserRoleAsync")]
        public async Task<IActionResult> RemoveUserRoleAsync(string id, Roles role)
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
            catch(RemoveUserRoleException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("DeletedUser/{id}")]
        public async Task<IActionResult> Deleted(string id)
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
