using IshTap.Business.DTOs.JobType;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class JobTypeService : IJobTypeService
{
    private readonly IJobTypeRepository _jobTypeRepository;

    public JobTypeService(IJobTypeRepository jobTypeRepository)
    {
        _jobTypeRepository = jobTypeRepository;
    }


    public async Task<List<JobType>> FindAllAsync()
    {
        var jobTypes = await _jobTypeRepository.FindAll().ToListAsync();
        if (jobTypes.Count==0)
        {
            throw new NotFoundException("Empty");
        }
        return jobTypes;
    }

    //crude
    public async Task CreateAsync(JobTypeCreateDto type)
    {
        if (type == null)
        {
            throw new ArgumentNullException("null argument");
        }

        JobType edu = new()
        {
            Type = type.jobType,
        };
        await _jobTypeRepository.CreateAsync(edu);
        await _jobTypeRepository.SaveAsync();
    }

    public async Task Delete(int id)
    {
        var jobType = await _jobTypeRepository.FindByIdAsync(id);
        if (jobType == null)
        {
            throw new NotFoundException("Null");
        }
        _jobTypeRepository.Delete(jobType);
        await _jobTypeRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, JobTypeUpdateDto jobType)
    {
        var baseType = await _jobTypeRepository.FindByIdAsync(id);
        if (baseType == null)
        {
            throw new NotFoundException("Null");
        }
        if (jobType == null)
        {
            throw new ArgumentNullException("Null argument");
        }
        baseType.Type = jobType.jobType;
        _jobTypeRepository.Update(baseType);
        await _jobTypeRepository.SaveAsync();
    }

}
