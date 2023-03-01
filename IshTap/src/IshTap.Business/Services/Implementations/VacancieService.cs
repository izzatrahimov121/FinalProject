using AutoMapper;
using IshTap.Business.DTOs.Vacancie;
using IshTap.Business.Exceptions;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace IshTap.Business.Services.Implementations;

public class VacancieService : IVacancieService
{
    //private readonly IWebHostEnvironment _env;
    private readonly IHostingEnvironment _env;
    private readonly IFileService _fileService;
    private readonly IVacancieRepository _vacancieRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IJobTypeRepository _jobTypeRepository;
    private readonly IMapper _mapper;

    public VacancieService(IVacancieRepository vacancieRepository,
                           IMapper mapper,
                           ICategoryRepository categoRyepository,
                           IJobTypeRepository jobTypeRepository,
                           IHostingEnvironment env,
                           IFileService fileService)
    {
        _vacancieRepository = vacancieRepository;
        _mapper = mapper;
        _categoryRepository = categoRyepository;
        _jobTypeRepository = jobTypeRepository;
        _env = env;
        _fileService = fileService;
    }




    public async Task<List<VacancieDto>> FindAllAsync(int skipt, int take)
    {
        var vacancies = await _vacancieRepository.FindAll().Where(v => v.IsActive == true).Skip(skipt).Take(take).ToListAsync();
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
                ContactEmail = vacancie.ContactEmail,
                ContactPhone = vacancie.ContactPhone,
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
    public async Task<VacancieDto?> FindByIdAsync(int id)
    {
        var vacancie = await _vacancieRepository.FindByIdAsync(id);
        if (vacancie == null)
        {
            throw new NotFoundException("not found");
        }
        vacancie.Views += 1;
        //var result = _mapper.Map<VacancieDto>(vacancie);
        _vacancieRepository.Update(vacancie);
        await _vacancieRepository.SaveAsync();
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
            ContactEmail = vacancie.ContactEmail,
            ContactPhone = vacancie.ContactPhone,
            JobDesctiption = vacancie.JobDesctiption,
            Responsibility = vacancie.Responsibility,
            IsActive = vacancie.IsActive,
            Views = vacancie.Views,
            Category = category?.Name,
            JobType = jobtype?.Type
        };
        return result;
    }
    public async Task<List<VacancieDto>> FindByConditionAsync(Expression<Func<Vacancie, bool>> expression)
    {
        var vacancies = await _vacancieRepository.FindByCondition(expression).ToListAsync();
        if (vacancies.Count==0)
        {
            throw new NotFoundException("Vacancies not found");
        }
        var result = _mapper.Map<List<VacancieDto>>(vacancies);
        return result;
    }




    //Crud
    public async Task CreateAsync(string userId, VacancieCreateDto vacancie)
    {
        if (vacancie is null) throw new ArgumentNullException(nameof(vacancie));
        //var resultCourse = _mapper.Map<Vacancie>(course);
        var category = await _categoryRepository.FindByIdAsync(vacancie.CategoryId);
        if (category is null) { throw new NotFoundException("Category not found"); }
        var jobtype = await _jobTypeRepository.FindByIdAsync(vacancie.JobTypeId);
        if (jobtype is null) { throw new NotFoundException("Job Type not found"); }
        string fileName = String.Empty;
        if (vacancie.Image != null)
        {
            fileName = await _fileService.CopyFileAsync(vacancie.Image, _env.WebRootPath, "assets", "img", "vacancies");
        }
        Vacancie result = new()
        {
            Title = vacancie.Title,
            JobDesctiption = vacancie.JobDesctiption,
            PublishedOn = DateTime.Now,
            ExpireOn = DateTime.Now.AddDays(60),
            Image = fileName,
            Address = vacancie.Address,
            Responsibility = vacancie.Responsibility,
            JobTypeId = vacancie.JobTypeId,
            CategoryId = vacancie.CategoryId,
            UserId = userId,
            ContactPhone = vacancie.ContactPhone,
            ContactEmail = vacancie.ContactEmail,
            Salary = vacancie.Salary,
            IsActive = true,
        };
        category.UsesCount += 1;
        await _vacancieRepository.CreateAsync(result);
        await _vacancieRepository.SaveAsync();
    }
    public async Task Delete(int id)
    {
        var baseVacancie = await _vacancieRepository.FindByIdAsync(id);

        if (baseVacancie == null)
        {
            throw new NotFoundException("Not Found.");
        }

        _vacancieRepository.Delete(baseVacancie);
        await _vacancieRepository.SaveAsync();
    }
    public async Task UpdateAsync(int id, VacancieUpdateDto vacancie)
    {
        var baseVacancie = await _vacancieRepository.FindByIdAsync(id);

        if (baseVacancie == null)
        {
            throw new NotFoundException("Not Found.");
        }
        string fileName = String.Empty;
        if (vacancie.Image != null)
        {
            fileName = await _fileService.CopyFileAsync(vacancie.Image, _env.WebRootPath, "assets", "img", "vacancies");
        }

        baseVacancie.Title = vacancie.Title;
        baseVacancie.JobDesctiption = vacancie.JobDesctiption;
        baseVacancie.Image = fileName;
        baseVacancie.Address = vacancie.Address;
        baseVacancie.ContactEmail = vacancie.ContactEmail;
        baseVacancie.ContactPhone = vacancie.ContactPhone;
        baseVacancie.Responsibility = vacancie.Responsibility;
        baseVacancie.JobTypeId = vacancie.JobTypeId;
        baseVacancie.CategoryId = vacancie.CategoryId;
        baseVacancie.Salary = vacancie.Salary;

        _vacancieRepository.Update(baseVacancie);
        await _vacancieRepository.SaveAsync();
    }




    //Filter
    public async Task<List<VacancieDto>> FilterByCategoryAndJobTypeAsync(int? categoryId, int? jobtypeId)
    {
        Category category = new Category();
        JobType jobtype = new JobType();
        List<Vacancie> vacancies = new List<Vacancie>();
        vacancies = await _vacancieRepository.FindAll().Where(v => v.IsActive == true).ToListAsync();
        if (categoryId != null)
        {
            category = await _categoryRepository.FindByIdAsync((int)categoryId);
            if (category == null) { throw new ArgumentNullException(); }
            vacancies = await _vacancieRepository.FindAll().Where(v => v.CategoryId == category.Id).ToListAsync();
        }

        if (jobtypeId != null)
        {
            jobtype = await _jobTypeRepository.FindByIdAsync((int)jobtypeId);
            if (jobtype == null) { throw new ArgumentNullException(); }
            vacancies = await _vacancieRepository.FindAll().Where(v => v.JobTypeId == jobtype.Id).ToListAsync();
        }

        var resultVacancies = _mapper.Map<List<VacancieDto>>(vacancies);
        return resultVacancies;
    }

    public async Task<List<VacancieDto>> FilterByCategoryAsync(int categoryId)
    {
        var category = await _categoryRepository.FindByIdAsync(categoryId);
        if (category == null) { throw new NotFoundException("category not found"); }

        var vacancies = await _vacancieRepository.FindAll().Where(v => v.IsActive == true && v.CategoryId == category.Id).ToListAsync();
        var resultVacancies = _mapper.Map<List<VacancieDto>>(vacancies);
        return resultVacancies;
    }

    public async Task<List<VacancieDto>> FiterByDateAsync(int date)
    {
        var resultDate = DateTime.Now.AddDays(-date);
        var vacancies = await _vacancieRepository.FindAll().Where(v => v.IsActive == true && v.PublishedOn >= resultDate).ToListAsync();
        var resultVacancies = _mapper.Map<List<VacancieDto>>(vacancies);
        return resultVacancies;
    }

    public async Task<List<VacancieDto>> FilterByConditionAsync(int? categoryId, int? jobtypeId, int? minSalary, int? maxSalary)
    {
        Category category = new Category();
        JobType jobtype = new JobType();
        var vacancies = await _vacancieRepository.FindAll().Where(v => v.IsActive == true).ToListAsync();
        if (vacancies == null)
        {
            throw new Exception("Heç bir şey tapılmadı");
        }
        if (categoryId != null)
        {
            category = await _categoryRepository.FindByIdAsync((int)categoryId);
            if (category == null) { throw new ArgumentNullException(); }
            vacancies = await _vacancieRepository.FindAll().Where(v => v.CategoryId == category.Id).ToListAsync();
        }

        if (jobtypeId != null)
        {
            jobtype = await _jobTypeRepository.FindByIdAsync((int)jobtypeId);
            if (jobtype == null) { throw new ArgumentNullException(); }
            vacancies = await _vacancieRepository.FindAll().Where(v => v.JobTypeId == jobtype.Id).ToListAsync();
        }
        if (minSalary != null) { vacancies = vacancies.Where(v => v.Salary >= minSalary).ToList(); }
        if (maxSalary != null) { vacancies = vacancies.Where(v => v.Salary <= maxSalary).ToList(); }
        var result = _mapper.Map<List<VacancieDto>>(vacancies);
        return result;
    }
    //son 15 vacancie
    public async Task<List<VacancieDto>> LastVacanciesAsync(int count)
    {
        var vacancies = await _vacancieRepository.FindAll().Where(v => v.IsActive == true).ToListAsync();
        if (count>vacancies.Count)
        {
            count = vacancies.Count;
        }
        var lastVacancies = await _vacancieRepository.FindAll().Where(v => v.Id >= vacancies.Count - count).ToListAsync();
        var resultVacancies = _mapper.Map<List<VacancieDto>>(vacancies);
        return resultVacancies;
    }
}
