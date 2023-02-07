using IshTap.Business.DTOs.Vacancie;
using IshTap.Core.Entities;
using System.Linq.Expressions;

namespace IshTap.Business.Services.Interfaces;

public interface IVacancieService
{
    Task<List<VacancieDto>> FindAllAsync();
    Task<List<VacancieDto>> FindByConditionAsync(Expression<Func<Vacancie, bool>> expression);
    Task<List<Vacancie>> FilterByCategoryAndJobTypeAsync(int categoryId, int jobtypeId);
    Task<List<Vacancie>> FilterByCategoryAsync(int categoryId);
    Task<List<Vacancie>> FiterByDateAsync(int date);
    Task<Vacancie?> FindByIdAsync(int id);
    Task CreateAsync(VacancieCreateDto vacancie);
    Task UpdateAsync(int id, VacancieUpdateDto vacancie);
    Task Delete(int id);
}
