using IshTap.Business.DTOs.Experience;
using IshTap.Business.DTOs.JobType;
using IshTap.Core.Entities;

namespace IshTap.Business.Services.Interfaces;

public interface IExperienceService
{
    Task<List<Experiences>> FindAllAsync();

    Task CreateAsync(ExperienceCreateDto experience);
    Task UpdateAsync(int id, ExperienceUpdateDto experience);
    Task Delete(int id);
}
