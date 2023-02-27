using IshTap.Business.DTOs.ApplyJob;
using IshTap.Business.DTOs.Auth;
using IshTap.Business.DTOs.CV;
using IshTap.Business.DTOs.Vacancie;
using IshTap.Business.Exceptions;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class UserProfileService : IUserProfileService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IVacancieRepository _vacancieRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IJobTypeRepository _jobTypeRepository;
    private readonly IHostingEnvironment _env;
    private readonly IEducationRepository _educationRepository;
    private readonly IExperiencesRepository _experiencesRepository;
    private readonly IFileService _fileService;
    private readonly ICVRepository _cvRepository;
    private readonly AppDbContexts _contexts;
    public UserProfileService(UserManager<AppUser> userManager, 
                              IVacancieRepository vacancieRepository, 
                              ICategoryRepository categoryRepository, 
                              IJobTypeRepository jobTypeRepository, 
                              IHostingEnvironment env, 
                              IEducationRepository educationRepository, 
                              IExperiencesRepository experiencesRepository, 
                              IFileService fileService, 
                              ICVRepository cvRepository, 
                              AppDbContexts contexts)
    {
        _userManager = userManager;
        _vacancieRepository = vacancieRepository;
        _categoryRepository = categoryRepository;
        _jobTypeRepository = jobTypeRepository;
        _env = env;
        _educationRepository = educationRepository;
        _experiencesRepository = experiencesRepository;
        _fileService = fileService;
        _cvRepository = cvRepository;
        _contexts = contexts;
    }
    private DbSet<AppUser> _table => _contexts.Set<AppUser>();


    public async Task ChenceImageAsync(string userId,UserImageDto ImageDto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        string fileName = await _fileService.CopyFileAsync(ImageDto.Image, _env.WebRootPath, "assets", "img", "user");
        user.Image = fileName;
        _table.Update(user);
        await _contexts.SaveChangesAsync();
    }
    public async Task<List<VacancieDto>> UserVacanciesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var vacancies = await _vacancieRepository.FindAll().Where(v => v.UserId == user.Id).ToListAsync();
        List<VacancieDto> resultVacancies = new List<VacancieDto>();
        foreach (var vacancie in vacancies)
        {
            var category = await _categoryRepository.FindByIdAsync(vacancie.CategoryId);
            var jobtype = await _jobTypeRepository.FindByIdAsync(vacancie.JobTypeId);
            VacancieDto result = new()
            {
                Id = vacancie.Id,
                Image = vacancie.Image,
                Title = vacancie.Title,
                Address = vacancie.Address,
                Salary = vacancie.Salary,
                PublishedOn = vacancie.PublishedOn,
                ExpireOn = vacancie.ExpireOn,
                JobDesctiption = vacancie.JobDesctiption,
                Responsibility = vacancie.Responsibility,
                IsActive = vacancie.IsActive,
                Views = vacancie.Views,
                Category = category?.Name,
                JobType = jobtype?.Type
            };
            resultVacancies.Add(result);
        }
        return resultVacancies;
    }
    public async Task<List<CVDto>> UserCVsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var cvs = await _cvRepository.FindAll().Where(c => c.UserId == user.Id).ToListAsync();
        List<CVDto> resultCV = new List<CVDto>();
        foreach (var cv in cvs)
        {
            var categry = await _categoryRepository.FindByIdAsync(cv.CategoryId);
            var edu = await _educationRepository.FindByIdAsync(cv.EducationId);
            var exp = await _experiencesRepository.FindByIdAsync(cv.ExperienceId);
            CVDto result = new()
            {
                Name = cv.Name,
                Surname = cv.Surname,
                FatherName = cv.FatherName,
                Iamge = cv.Iamge,
                AboutYourself = cv.AboutYourself,
                Position = cv.Position,
                City = cv.City,
                MinSalary = cv.MinSalary,
                Skills = cv.Skills,
                Details = cv.Details,
                Email = cv.Email,
                Phone = cv.Phone,
                PublishedOn = cv.PublishedOn,
                ExpireOn = cv.ExpireOn,
                Views = cv.Views,
                IsActive = cv.IsActive,
                Education = edu?.Type,
                Category = categry?.Name,
                Experience = exp?.Type
            };
        }
        return resultCV;
    }

    public async Task ChanceUserInformation(string userId, ChanceUserInformation information)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) { throw new NotFoundException("User not found"); }
        if (information.NewEmail !=null)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
    }

}
