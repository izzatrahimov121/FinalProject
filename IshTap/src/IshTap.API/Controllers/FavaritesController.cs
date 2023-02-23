using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IshTap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavaritesController : ControllerBase
    {
        private readonly IFavoriteVacancieServices _favoriteVacancieServices;
        private readonly UserManager<AppUser> _userManager;

        public FavaritesController(IFavoriteVacancieServices favoriteVacancieServices, UserManager<AppUser> userManager)
        {
            _favoriteVacancieServices = favoriteVacancieServices;
            _userManager = userManager;
        }

        [HttpPost("add/{vacancieId}")]
        public async Task<IActionResult> AddFavorites(int vacancieId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if (user is null) { throw new NotFoundException("User not found"); }
                await _favoriteVacancieServices.AddFavoritesAsync(vacancieId, user.Id);
                return Ok();
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFovarite(int id)
        {
            try
            {
                await _favoriteVacancieServices.DeleteFavoritesAsync(id);
                return Ok();
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
