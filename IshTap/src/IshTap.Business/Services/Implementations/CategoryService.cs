using IshTap.Business.DTOs.Category;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoRyepository)
    {
        _categoryRepository = categoRyepository;
    }


    //crud
    public async Task CreateAsync(CategoryCreateDto category)
    {
        if (category is null) { throw new ArgumentNullException(); }
        Category result = new()
        {
            Name = category.Name,
            UsesCount = 0
        };
        await _categoryRepository.CreateAsync(result);
        await _categoryRepository.SaveAsync();
    }

    public async Task Delete(int id)
    {
        var category = await _categoryRepository.FindByIdAsync(id);
        if (category == null)
        {
            throw new NotFoundException("Not Found.");
        }

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, CategoryUpdateDto entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        var baseCategory = await _categoryRepository.FindByIdAsync(id);
        if (baseCategory == null)
        {
            throw new NotFoundException("Not Found.");
        }
        baseCategory.Name = entity.Name;
        _categoryRepository.Update(baseCategory);
        await _categoryRepository.SaveAsync();
    }




    public async Task<List<Category>> FindAllAsync()
    {
        var catogories = await _categoryRepository.FindAll().ToListAsync();
        return catogories;
    }

    public async Task<Category?> FindByIdAsync(int id)
    {
        var category = await _categoryRepository.FindByIdAsync(id);
        if (category == null)
        {
            throw new NotFoundException("not found");
        }
        return category;
    }

    public async Task<List<Category>> TopCategory(int count)
    {
        var categoryes = await _categoryRepository.FindAll().ToListAsync();
        if (categoryes is null)
        {
            throw new NotFoundException("Not Found");
        }
        if (categoryes.Count<count)
        {
            throw new NotFoundException("Not Found");
        }
        return categoryes.OrderByDescending(c => c.UsesCount).ToList().Take(count).ToList();
    }

}
