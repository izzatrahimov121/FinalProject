using IshTap.Business.DTOs.Vacancie;
using IshTap.Core.Entities;
using System.Linq.Expressions;

namespace IshTap.Business.Services.Interfaces;

public interface IVacancieService
{
    Task<List<VacancieDto>> FindAllAsync();
    Task<List<VacancieDto>> FindByConditionAsync(Expression<Func<Vacancie, bool>> expression);
    Task<List<Vacancie>> FilterByCategoryAndJobTypeAsync(int categoryId, int jobtypeId);
    Task<List<Vacancie>?> FilterByDateJobtypeCategoryAsync(int date = 60, int? jobtypeId = null, int? categoryId = null);
    Task<List<Vacancie>> FilterByCategoryAsync(int categoryId);
    Task<List<Vacancie>> FiterByDateAsync(int date);
    Task<List<Vacancie>> FilterByConditionAsync(int categoryId, int jobtypeId, int minSalary, int maxSalary);
    Task<List<Vacancie>> LastVacanciesAsync();
    Task<Vacancie?> FindByIdAsync(int id);
    Task CreateAsync(VacancieCreateDto vacancie);
    Task UpdateAsync(int id, VacancieUpdateDto vacancie);
    Task Delete(int id);
}
