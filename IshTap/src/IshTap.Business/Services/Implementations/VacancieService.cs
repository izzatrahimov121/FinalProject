﻿using AutoMapper;
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
    private readonly ICategoryRepository _categoRyepository;
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
        _categoRyepository = categoRyepository;
        _jobTypeRepository = jobTypeRepository;
        _env = env;
        _fileService = fileService;
    }




    public async Task<List<VacancieDto>> FindAllAsync()
    {
        var vacancies = await _vacancieRepository.FindAll().ToListAsync();
        foreach (var vacancie in vacancies)
        {
            if (vacancie.IsActive == true && vacancie.PlacamentTime <= DateTime.Now.AddDays(-60))
            {
                vacancie.IsActive = false;
                _vacancieRepository.Update(vacancie);
            }
        }
        await _vacancieRepository.SaveAsync();
        var resultVacancies = _mapper.Map<List<VacancieDto>>(vacancies);
        return resultVacancies;
    }
    public async Task<Vacancie?> FindByIdAsync(int id)
    {
        var vacancie = await _vacancieRepository.FindByIdAsync(id);
        if (vacancie == null)
        {
            throw new NotFoundException("not found");
        }
        vacancie.Review += 1;
        //var result = _mapper.Map<VacancieDto>(vacancie);
        _vacancieRepository.Update(vacancie);
        await _vacancieRepository.SaveAsync();
        return vacancie;
    }
    public async Task<List<VacancieDto>> FindByConditionAsync(Expression<Func<Vacancie, bool>> expression)
    {
        var courses = await _vacancieRepository.FindByCondition(expression).ToListAsync();
        var result = _mapper.Map<List<VacancieDto>>(courses);
        return result;
    }




    //Crud
    public async Task CreateAsync(VacancieCreateDto vacancie)
    {
        if (vacancie is null) throw new ArgumentNullException(nameof(vacancie));
        //var resultCourse = _mapper.Map<Vacancie>(course);
        string fileName = await _fileService.CopyFileAsync(vacancie.Image, _env.WebRootPath, "assets","img","vacancies");

        Vacancie result = new()
        {
            Title = vacancie.Title,
            JobDesctiption = vacancie.JobDesctiption,
            PlacamentTime = DateTime.Now,
            Image = fileName,
            Address = vacancie.Address,
            Responsibility = vacancie.Responsibility,
            JobTypeId = vacancie.JobTypeId,
            CategoryId = vacancie.CategoryId,
            Salary = vacancie.Salary,
            IsActive = true
        };

        var category = await _categoRyepository.FindByIdAsync(vacancie.CategoryId);
        category.UsesCount += 1;
        await _vacancieRepository.CreateAsync(result);
        await _vacancieRepository.SaveAsync();
    }


    public async Task Delete(int id)
    {
        var baseCourse = await _vacancieRepository.FindByIdAsync(id);

        if (baseCourse == null)
        {
            throw new NotFoundException("Not Found.");
        }

        _vacancieRepository.Delete(baseCourse);
        await _vacancieRepository.SaveAsync();
    }


    public async Task UpdateAsync(int id, VacancieUpdateDto vacancie)
    {
        var baseVacancie = await _vacancieRepository.FindByIdAsync(id);

        if (baseVacancie == null)
        {
            throw new NotFoundException("Not Found.");
        }
        if (vacancie is null)
        {
            throw new ArgumentNullException(nameof(vacancie));
        }

        string fileName = await _fileService.CopyFileAsync(vacancie.Image, _env.WebRootPath, "assets", "img", "vacancies");

        baseVacancie.Title = vacancie.Title;
        baseVacancie.JobDesctiption = vacancie.JobDesctiption;
        baseVacancie.Image = fileName;
        baseVacancie.Address = vacancie.Address;
        baseVacancie.Responsibility = vacancie.Responsibility;
        baseVacancie.JobTypeId = vacancie.JobTypeId;
        baseVacancie.CategoryId = vacancie.CategoryId;
        baseVacancie.Salary = vacancie.Salary;

        _vacancieRepository.Update(baseVacancie);
        await _vacancieRepository.SaveAsync();
    }






    //Filter
    public async Task<List<Vacancie>> FilterByCategoryAndJobTypeAsync(int categoryId, int jobtypeId)
    {
        var category = await _categoRyepository.FindByIdAsync(categoryId);
        if (category == null) { throw new ArgumentNullException(); }

        var jobtype = await _jobTypeRepository.FindByIdAsync(jobtypeId);
        if (jobtype == null) { throw new ArgumentNullException(); }


        var vacancies = await _vacancieRepository.FindAll().Where(v => v.CategoryId == category.Id && v.JobTypeId == jobtype.Id).ToListAsync();
        return vacancies;
    }

    public async Task<List<Vacancie>?> FilterByDateJobtypeCategoryAsync(int date, int? jobtypeId = null, int? categoryId = null)
    {
        var resultDate = DateTime.Now.AddDays(-date);
        var vacancies = await _vacancieRepository.FindAll().Where(v => v.IsActive == true
                                                                    && v.PlacamentTime >= resultDate
                                                                    && v.JobTypeId == jobtypeId
                                                                    && v.CategoryId == categoryId).ToListAsync();
        return vacancies;
    }

    public async Task<List<Vacancie>> FilterByCategoryAsync(int categoryId)
    {
        var category = await _categoRyepository.FindByIdAsync(categoryId);
        if (category == null) { throw new ArgumentNullException(); }

        var vacancies = await _vacancieRepository.FindAll().Where(v => v.CategoryId == category.Id).ToListAsync();
        return vacancies;
    }

    public async Task<List<Vacancie>> FiterByDateAsync(int date)
    {
        var resultDate = DateTime.Now.AddDays(-date);
        var vacancies = await _vacancieRepository.FindAll().Where(v => v.PlacamentTime >= resultDate).ToListAsync();
        return vacancies;
    }

    public async Task<List<Vacancie>> FilterByConditionAsync(int categoryId, int jobtypeId, int minSalary, int maxSalary)
    {
        var activeVacancies = await _vacancieRepository.FindAll().Where(v => v.IsActive == true).ToListAsync();
        if (activeVacancies == null)
        {
            throw new Exception("Heç bir şey tapılmadı");
        }

        var result = await _vacancieRepository.FindAll().Where(r => r.CategoryId == categoryId
                                                                 && r.JobTypeId == jobtypeId
                                                                 && r.Salary>=minSalary
                                                                 && r.Salary<=maxSalary).ToListAsync();

        return result;
    }

    //son 15 vacancie
    public async Task<List<Vacancie>> LastVacanciesAsync()
    {
        var vacancies = await _vacancieRepository.FindAll().Where(v => v.IsActive == true).ToListAsync();
        var lastVacancies = await _vacancieRepository.FindAll().Where(v => v.Id >= vacancies.Count - 15).ToListAsync();
        return vacancies;
    }


}
