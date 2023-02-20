using IshTap.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using Microsoft.EntityFrameworkCore;
using IshTap.Core.Enums;
using IshTap.DataAccess.Repository.Interfaces;

namespace IshTap.DataAccess.Contexts;

public class AppDbContextInitializer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContexts _context;
    private readonly IEducationRepository _educationRepository;
    private readonly IExperiencesRepository _experiencesRepository;
    private readonly IJobTypeRepository _jobTypeRepository;
    private readonly ICategoryRepository _categoryRepository;

    public AppDbContextInitializer(UserManager<AppUser> userManager
        , RoleManager<IdentityRole> roleManager
        , IConfiguration configuration
        , AppDbContexts context
        , IEducationRepository educationRepository
        , IExperiencesRepository experiencesRepository
        , IJobTypeRepository jobTypeRepository
        , ICategoryRepository categoryRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _context = context;
        _educationRepository = educationRepository;
        _experiencesRepository = experiencesRepository;
        _jobTypeRepository = jobTypeRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.MigrateAsync();
    }

    public async Task CategorySeedAsync()
    {
        var categorys = new List<Category>()
        {
            new Category(){Name="IT"},
            new Category(){Name="System Administration"},
            new Category(){Name="Database Development and Management"},
            new Category(){Name="Programming"},
            new Category(){Name="Hardware Specialist"},
            new Category(){Name="IT Project Management"},
        };

        foreach (var cate in categorys)
        {
            await _categoryRepository.CreateAsync(cate);
        }
    }
    public async Task ExperiencesSeedAsync()
    {
        var experiences = new List<Experiences>() {
                new Experiences(){ Type="Less than 1 year" },
                new Experiences(){ Type="From 1 to 2 years"},
                new Experiences(){ Type="From 2 to 3 years"},
                new Experiences(){ Type="More than 5 years"}
            };
        foreach (var exp in experiences)
        {
            await _experiencesRepository.CreateAsync(exp);
        }
    }
    public async Task JobTypeSeedAsync()
    {
        var jobtyps = new List<JobType>() {
                new JobType(){ Type="Part-Time" },
                new JobType(){ Type="Full-Time"}
        };
        foreach (var type in jobtyps)
        {
            await _jobTypeRepository.CreateAsync(type);
        }
    }
    public async Task EducationSeedAsync()
    {
        var edus = new List<Educations>()
        {
            new Educations(){Type="Scientific degree"},
            new Educations(){Type="Higher"},
            new Educations(){Type="Incomplete Higher"},
            new Educations(){Type="Secondary Technical"},
            new Educations(){Type="Specialized Secondary"},
            new Educations(){Type="Secondary"},
        };
        foreach (var edu in edus) 
        {
            await _educationRepository.CreateAsync(edu);
        }
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
        };

        await _userManager.CreateAsync(admin, _configuration["AdminSettings:Password"]);
        await _userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
    }
}
