using IshTap.Business.DTOs.Auth;
using IshTap.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UserProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetUserInfoFromLogined()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                UserInfoDto userInfo = new()
                {
                    UserName = user.UserName,
                    Fullname = user.Fullname,
                    Email = user.Email,
                };
                return Ok(userInfo);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

        }
    }
}
