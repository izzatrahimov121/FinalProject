using IshTap.Business.DTOs.Auth;
using IshTap.Core.Enums;

namespace IshTap.Business.Services.Interfaces;

public interface IUserManagerService
{
    Task CreateAsync(RegisterDto registerDto, Roles roles);
    Task UpdateUserRoleAsync(string id, Roles role);
    Task AddUserRoleAsync(string id, Roles role);
    Task RemoveUserRoleAsync(string id, Roles role);
    Task DeleteUser(string id);
    Task<List<AppUserDto>> FindAllAsync();
    Task<AppUserDto?> FindByIdAsync(string id);
}
