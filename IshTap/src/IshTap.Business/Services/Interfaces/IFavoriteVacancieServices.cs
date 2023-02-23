using IshTap.Business.DTOs.Vacancie;

namespace IshTap.Business.Services.Interfaces;

public interface IFavoriteVacancieServices
{
    Task AddFavoritesAsync(int vacancieId, string userId);
    Task DeleteFavoritesAsync(int id);
    Task<List<VacancieDto>> Favorites(string userId);
}
