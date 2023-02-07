using IshTap.Business.DTOs.Category;
using IshTap.Business.DTOs.Vacancie;
using IshTap.Core.Entities;
using System.Linq.Expressions;

namespace IshTap.Business.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> FindAllAsync();
    Task<Category?> FindByIdAsync(int id);

    Task CreateAsync(CategoryCreateDto category);
    Task UpdateAsync(int id, CategoryUpdateDto category);
    Task Delete(int id);
}
