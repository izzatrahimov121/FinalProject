using IshTap.Business.DTOs.Vacancie;
using IshTap.Core.Entities;
using System.Linq.Expressions;

namespace IshTap.Business.Services.Interfaces;

public interface IVacancieService
{
    Task<List<VacancieDto>> FindAllAsync();
    Task<List<VacancieDto>> FindByConditionAsync(Expression<Func<Vacancie, bool>> expression);
    Task<List<VacancieDto>> FilterByCategoryAndJobTypeAsync(int? categoryId, int? jobtypeId);
    Task<List<VacancieDto>> FilterByCategoryAsync(int categoryId);
    Task<List<VacancieDto>> FiterByDateAsync(int date);
    Task<List<VacancieDto>> FilterByConditionAsync(int? categoryId, int? jobtypeId, int? minSalary, int? maxSalary);
    Task<List<VacancieDto>> LastVacanciesAsync();
    Task<VacancieDto?> FindByIdAsync(int id);
    Task CreateAsync(string userId, VacancieCreateDto vacancie);
    Task UpdateAsync(int id, VacancieUpdateDto vacancie);
    Task Delete(int id);
}
