using IshTap.Business.DTOs.GetInTouch;

namespace IshTap.Business.Services.Interfaces;

public interface IGetInTouchService
{
    Task CreateAsync(string userId, GetInTouchDto getInTouchDto);
    Task<List<GetInTouchDto>> AllMessages();
    Task<GetInTouchDto> Message(int id);
    Task Delete(int id);
}
