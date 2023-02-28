using AutoMapper;
using IshTap.Business.DTOs.Vacancie;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Implementations;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class FavoriteVacancieServices : IFavoriteVacancieServices
{
    private readonly AppDbContexts _contexts;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IJobTypeRepository _jobTypeRepository;
    private readonly IVacancieRepository _vacancieRepository;
    private readonly IMapper _mapper;
    public FavoriteVacancieServices(UserManager<AppUser> userManager, AppDbContexts contexts, IMapper mapper,
        IVacancieRepository vacancieRepository, ICategoryRepository categoryRepository, IJobTypeRepository jobTypeRepository)
    {
        _userManager = userManager;
        _contexts = contexts;
        _mapper = mapper;
        _vacancieRepository = vacancieRepository;
        _categoryRepository = categoryRepository;
        _jobTypeRepository = jobTypeRepository;
    }
    private DbSet<FavoriteVacancies> _table => _contexts.Set<FavoriteVacancies>();

    public async Task AddFavoritesAsync(int vacancieId, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) { throw new NotFoundException("User not found"); }
        var vacancie = await _vacancieRepository.FindByIdAsync(vacancieId);
        if (vacancie == null) { throw new NotFoundException("Vacancie not found"); }
        var controle = await _table.AsQueryable().AsNoTracking().Where(v => v.UserId == user.Id && v.VacancieId==vacancieId).ToListAsync();
        if (controle.Count>=1)
        {
            throw new BadRequestException("The vacancy has already been added");
        }
        FavoriteVacancies favorite = new()
        {
            VacancieId = vacancie.Id,
            UserId = userId,
        };
        await _table.AddAsync(favorite);
        await _contexts.SaveChangesAsync();
    }
    public async Task DeleteFavoritesAsync(int id)
    {
        var favorite = await _table.FindAsync(id);
        if (favorite == null) { throw new NotFoundException("Not Found"); }
        _table.Remove(favorite);
        await _contexts.SaveChangesAsync();
    }
    public async Task<List<VacancieDto>> Favorites(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) { throw new NotFoundException("User not found"); }
        var fovarites = await _table.AsQueryable().AsNoTracking().Where(v => v.UserId == user.Id).ToListAsync();
        List<Vacancie> vacancies = new List<Vacancie>();
        foreach (var fovarite in fovarites)
        {
            var vacancie = await _vacancieRepository.FindByIdAsync(fovarite.VacancieId);
            if (vacancie.IsActive == true)
            {
                vacancies.Add(vacancie);
            }
        }
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
}
