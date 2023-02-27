using IshTap.Business.DTOs.Category;
using IshTap.Business.DTOs.Education;
using IshTap.Core.Entities;

namespace IshTap.Business.Services.Interfaces;

public interface IEducationService
{
    Task<List<Educations>> FindAllAsync();

    //crude
    Task CreateAsync(EducationCreateDto education);
    Task UpdateAsync(int id, EducationUpdateDto education);
    Task Delete(int id);
}
