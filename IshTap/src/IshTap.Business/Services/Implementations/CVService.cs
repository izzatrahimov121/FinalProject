using AutoMapper;
using IshTap.Business.DTOs.CV;
using IshTap.Business.Exceptions;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class CVService : ICVService
{
    private readonly ICVRepository _cvRepository;
    private readonly IFileService _fileService;
    private readonly IEducationRepository _educationRepository;
    private readonly IExperiencesRepository _experiencesRepository;
    private readonly IHostingEnvironment _env;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CVService(ICVRepository cvRepository
                   , IFileService fileService
                   , IHostingEnvironment env
                   , ICategoryRepository categoryRepository
                   , IMapper mapper
                   , IEducationRepository educationRepository
                   , IExperiencesRepository experiencesRepository)
    {
        _cvRepository = cvRepository;
        _fileService = fileService;
        _env = env;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _educationRepository = educationRepository;
        _experiencesRepository = experiencesRepository;
    }

    public async Task CreateAsync(string userId, CVCreatedDto cv)
    {
        if (cv is null) throw new ArgumentNullException(nameof(cv));

        string fileName = String.Empty;
        if (cv.Iamge != null)
        {
            fileName = await _fileService.CopyFileAsync(cv.Iamge, _env.WebRootPath, "assets", "img", "CVs");
        }

        CVs result = new()
        {
            Name = cv.Name,
            Surname = cv.Surname,
            FatherName = cv.FatherName,
            Iamge = fileName,
            AboutYourself = cv.AboutYourself,
            Position = cv.Position,
            MinSalary = cv.MinSalary,
            City = cv.City,
            Skills = cv.Skills,
            Details = cv.Details,
            Email = cv.Email,
            Phone = cv.Phone,
            CategoryId = cv.CategoryId,
            EducationId = cv.EducationId,
            ExperienceId = cv.ExperienceId,
            UserId = userId,
            PublishedOn = DateTime.Now,
            ExpireOn = DateTime.Now.AddDays(30),
        };
        var category = await _categoryRepository.FindByIdAsync(cv.CategoryId);
        category.UsesCount += 1;
        await _cvRepository.CreateAsync(result);
        await _cvRepository.SaveAsync();
    }
    public async Task UpdateAsync(int id, CVUpdateDto cv)
    {
        var baseCv = await _cvRepository.FindByIdAsync(id);

        if (baseCv == null)
        {
            throw new NotFoundException("Not Found.");
        }
        if (cv is null)
        {
            throw new ArgumentNullException(nameof(cv));
        }

        string fileName = await _fileService.CopyFileAsync(cv.Iamge, _env.WebRootPath, "assets", "img", "CVs");

        baseCv.Name = cv.Name;
        baseCv.Surname = cv.Surname;
        baseCv.FatherName = cv.FatherName;
        baseCv.Iamge = fileName;
        baseCv.AboutYourself = cv.AboutYourself;
        baseCv.Position = cv.Position;
        baseCv.MinSalary = cv.MinSalary;
        baseCv.City = cv.City;
        baseCv.Skills = cv.Skills;
        baseCv.Details = cv.Details;
        baseCv.Email = cv.Email;
        baseCv.Phone = cv.Phone;
        baseCv.CategoryId = cv.CategoryId;
        baseCv.EducationId = cv.EducationId;
        baseCv.ExperienceId = cv.ExperienceId;

        _cvRepository.Update(baseCv);
        await _cvRepository.SaveAsync();
    }
    public async Task Delete(int id)
    {
        var cv = await _cvRepository.FindByIdAsync(id);
        if (cv is null)
        {
            throw new ArgumentNullException(nameof(cv));
        }
        _cvRepository.Delete(cv);
        await _cvRepository.SaveAsync();
    }
    public async Task<List<CVDto>> FindAllAsync()
    {
        var cvs = await _cvRepository.FindAll().Where(c=>c.IsActive==true).ToListAsync();
        //foreach (var cv in cvs)
        //{
        //    if (cv.IsActive == true && DateTime.Now >= cv.ExpireOn)
        //    {
        //        cv.IsActive = false;
        //        _cvRepository.Update(cv);
        //    }
        //}
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
            resultCV.Add(result);
        }
        return resultCV;
    }
    public async Task<CVDto?> FindByIdAsync(int id)
    {
        var cv = await _cvRepository.FindByIdAsync(id);
        if (cv == null)
        {
            throw new NotFoundException("not found");
        }
        cv.Views += 1;
        _cvRepository.Update(cv);
        await _cvRepository.SaveAsync();
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
        return result;
    }
    public async Task<List<CVDto>> LastVacanciesAsync()
    {
        var cvs = await _cvRepository.FindAll().Where(c => c.IsActive == true).ToListAsync();
        var lastCv = await _cvRepository.FindAll().Where(c => c.Id >= cvs.Count - 15).ToListAsync();
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
            resultCV.Add(result);
        }
        return resultCV;
    }
}
