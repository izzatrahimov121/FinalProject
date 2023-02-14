using IshTap.Business.DTOs.Auth;
using IshTap.Core.Entities;

namespace IshTap.Business.HelperServices.Interfaces;

public interface ITokenHandler
{
    Task<TokenResponseDto> GenerateTokenAsync(AppUser user, int minute);
}
