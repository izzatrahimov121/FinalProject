using IshTap.Business.DTOs.Education;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class EducationService : IEducationService
{
    private readonly IEducationRepository _educationRepository;

    public EducationService(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }



    public async Task<List<Educations>> FindAllAsync()
    {
        var educationTypes = await _educationRepository.FindAll().ToListAsync();
        if (educationTypes.Count==0)
        {
            throw new NotFoundException("Empty");
        }
        return educationTypes;
    }


    //crude
    public async Task CreateAsync(EducationCreateDto education)
    {
        if (education == null)
        {
            throw new ArgumentNullException("null argument");
        }

        Educations edu = new()
        {
            Type = education.EducationType,
        };
        await _educationRepository.CreateAsync(edu);
        await _educationRepository.SaveAsync();
    }

    public async Task Delete(int id)
    {
        var educationType = await _educationRepository.FindByIdAsync(id);
        if (educationType == null)
        {
            throw new ArgumentNullException("Null");
        }
        _educationRepository.Delete(educationType);
        await _educationRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, EducationUpdateDto education)
    {
        var baseEducation = await _educationRepository.FindByIdAsync(id);
        if (baseEducation == null)
        {
            throw new NotFoundException("Not found");
        }
        if (education == null)
        {
            throw new ArgumentNullException("Null argument");
        }
        baseEducation.Type = education.EducationType;
        _educationRepository.Update(baseEducation);
        await _educationRepository.SaveAsync();
    }
}
