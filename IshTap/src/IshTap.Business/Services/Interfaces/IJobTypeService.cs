using IshTap.Business.DTOs.JobType;
using IshTap.Core.Entities;

namespace IshTap.Business.Services.Interfaces;

public interface IJobTypeService
{
    Task<List<JobType>> FindAllAsync();
    Task<JobType?> FindByIdAsync(int id);

    Task CreateAsync(JobTypeCreateDto jobType);
    Task UpdateAsync(int id, JobTypeUpdateDto category);
    Task Delete(int id);
}
