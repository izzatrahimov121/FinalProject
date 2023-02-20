using IshTap.Business.DTOs.Auth;
using IshTap.Business.DTOs.CV;
using IshTap.Business.DTOs.Vacancie;

namespace IshTap.Business.Services.Interfaces;

public interface IUserProfileService
{
    Task ChenceImageAsync(string userId, UserImageDto Image);
    Task<List<VacancieDto>> UserVacanciesAsync(string userId);
    Task<List<CVDto>> UserCVsAsync(string userId);
}
