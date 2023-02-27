using AutoMapper;
using IshTap.Business.DTOs.Auth;
using IshTap.Business.DTOs.GetInTouch;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Repository.Interfaces;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class GetInTouchService : IGetInTouchService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IGetInTouchRepository _getInTouchRepository;
    private readonly IMapper _mapper;

    public GetInTouchService(UserManager<AppUser> userManager,
                             IGetInTouchRepository getInTouchRepository,
                             IMapper mapper)
    {
        _userManager = userManager;
        _getInTouchRepository = getInTouchRepository;
        _mapper = mapper;
    }

    public async Task CreateAsync(string userId, GetInTouchDto getInTouchDto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) { throw new NotFoundException("User not found"); }
        if (getInTouchDto is null) { throw new ArgumentNullException(nameof(getInTouchDto)); }
        GetInTouch result = new()
        {
            Name = getInTouchDto.Name,
            Message = getInTouchDto.Message,
            Subject = getInTouchDto.Subject,
            Email = getInTouchDto.Email,
            UserId = userId,
        };
        await _getInTouchRepository.CreateAsync(result);
        await _getInTouchRepository.SaveAsync();
    }

    public async Task<List<GetInTouchDto>> AllMessages()
    {
        var messages = await _getInTouchRepository.FindAll().ToListAsync();
        var result = _mapper.Map<List<GetInTouchDto>>(messages);
        return result;
    }

    public async Task<GetInTouchDto> Message(int id)
    {
        var message = await _getInTouchRepository.FindByIdAsync(id);
        if (message == null) { throw new NotFoundException("Not found"); }
        var result = _mapper.Map<GetInTouchDto>(message);
        return result;
    }

    public async Task Delete(int id)
    {
        var message = await _getInTouchRepository.FindByIdAsync(id);
        if (message == null) { throw new NotFoundException("Message not found"); }
        _getInTouchRepository.Delete(message);
        await _getInTouchRepository.SaveAsync();
    }

}
