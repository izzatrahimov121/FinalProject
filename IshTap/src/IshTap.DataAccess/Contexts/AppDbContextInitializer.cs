using IshTap.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using Microsoft.EntityFrameworkCore;
using IshTap.Core.Enums;

namespace IshTap.DataAccess.Contexts;

public class AppDbContextInitializer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContexts _context;

    public AppDbContextInitializer(UserManager<AppUser> userManager
        , RoleManager<IdentityRole> roleManager
        , IConfiguration configuration
        , AppDbContexts context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.MigrateAsync();
    }

    public async Task RoleSeedAsync()
    {
        foreach (var role in Enum.GetValues(typeof(Roles)))
        {
            if (!await _roleManager.RoleExistsAsync(role.ToString()))
            {
                await _roleManager.CreateAsync(new() { Name = role.ToString() });
            }
        }
    }

    public async Task UserSeedAsync()
    {
        AppUser admin = new AppUser
        {
            Fullname = _configuration["AdminSettings:Fullname"],
            UserName = _configuration["AdminSettings:UserName"],
            Email = _configuration["AdminSettings:Email"],
            IsActive= true
        };

        await _userManager.CreateAsync(admin, _configuration["AdminSettings:Password"]);
        await _userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
    }
}
