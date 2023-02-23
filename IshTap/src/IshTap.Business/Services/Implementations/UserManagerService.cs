using IshTap.Business.DTOs.Auth;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.Core.Enums;
using IshTap.DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class UserManagerService : IUserManagerService
{
    private readonly AppDbContexts _contexts;

    private readonly UserManager<AppUser> _userManager;
    public DbSet<AppUser> _table => _contexts.Set<AppUser>();
    public UserManagerService(AppDbContexts contexts, UserManager<AppUser> userManager)
    {
        _contexts = contexts;
        _userManager = userManager;
    }


    public async Task<List<AppUserDto>> FindAllAsync()
    {
        var users = await _table.AsQueryable().AsNoTracking().ToListAsync();
        List<AppUserDto> userDto = new List<AppUserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            AppUserDto appuser = new()
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Image = user.Image,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles.ToList(),
            };
            userDto.Add(appuser);
        }
        return userDto;
    }
    public async Task<AppUserDto?> FindByIdAsync(string id)
    {
        var baseUser = await _table.FindAsync(id);
        if (baseUser is null) throw new ArgumentNullException("User is not");
        var roles = await _userManager.GetRolesAsync(baseUser);
        AppUserDto appuser = new()
        {
            Id = baseUser.Id,
            Fullname = baseUser.Fullname,
            Image = baseUser.Image,
            Username = baseUser.UserName,
            Email = baseUser.Email,
            Roles = roles.ToList(),
        };
        return appuser;
    }
    public async Task CreateAsync(RegisterDto registerDto, Roles role)
    {
        AppUser user = new()
        {
            Email = registerDto.Email,
            UserName = registerDto.Username,
            Fullname = registerDto.Fullname,
        };

        var identityResult = await _userManager.CreateAsync(user, registerDto.Password);
        if (!identityResult.Succeeded)
        {
            string errors = String.Empty;
            int count = 0;
            foreach (var error in identityResult.Errors)
            {
                errors += count != 0 ? $",{error.Description}" : $"{error.Description}";
                count++;
            }
            throw new UserCreateFailException(errors);
        }

        var result = await _userManager.AddToRoleAsync(user, role.ToString());
        if (!result.Succeeded)
        {
            string errors = String.Empty;
            int count = 0;
            foreach (var error in result.Errors)
            {
                errors += count != 0 ? $",{error.Description}" : $"{error.Description}";
                count++;
            }
            throw new RoleCreateFailException(errors);
        }
    }
    public async Task UpdateUserRoleAsync(string id, Roles role)
    {
        var user = await _table.FindAsync(id);
        if (user is null) throw new ArgumentNullException("User is not");
        var curretRole = await _userManager.GetRolesAsync(user);
        await _userManager.AddToRoleAsync(user, role.ToString());
        await _userManager.RemoveFromRoleAsync(user, curretRole.ToString());
        await _contexts.SaveChangesAsync();
    }
    public async Task AddUserRoleAsync(string id, Roles role)
    {
        var user = await _table.FindAsync(id);
        if (user is null) throw new ArgumentNullException("User is not");
        await _userManager.AddToRoleAsync(user, role.ToString());
        await _contexts.SaveChangesAsync();
    }
    public async Task RemoveUserRoleAsync(string id, Roles role)
    {
        var user = await _table.FindAsync(id);
        if (user is null) throw new ArgumentNullException("User is not");
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.ToList().Count <= 1)
        {
            throw new RemoveUserRoleException("User has only 1 role. Try changing role");
        }
        await _userManager.RemoveFromRoleAsync(user, role.ToString());
        await _contexts.SaveChangesAsync();
    }
    public async Task DeleteUser(string id)
    {
        var user = await _table.FindAsync(id);
        if (user is null) throw new ArgumentNullException("User is not");
        _table.Remove(user);
        await _contexts.SaveChangesAsync();
    }
}
