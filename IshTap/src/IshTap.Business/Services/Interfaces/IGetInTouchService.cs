using IshTap.Business.DTOs.GetInTouch;

namespace IshTap.Business.Services.Interfaces;

public interface IGetInTouchService
{
    Task CreateAsync(string userId, GetInTouchDto getInTouchDto);
    Task<List<GetInTouchDto>> Messages();
    Task Delete(int id);
}
