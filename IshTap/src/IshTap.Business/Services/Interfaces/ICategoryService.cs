using IshTap.Business.DTOs.Category;
using IshTap.Core.Entities;

namespace IshTap.Business.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> FindAllAsync();
    Task<Category?> FindByIdAsync(int id);
    Task<List<Category>> TopCategory(int count);

    Task CreateAsync(CategoryCreateDto category);
    Task UpdateAsync(int id, CategoryUpdateDto category);
    Task Delete(int id);
}
