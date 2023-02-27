using IshTap.Business.DTOs.Education;
using IshTap.Business.DTOs.Experience;
using IshTap.Business.Exceptions;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Repository.Implementations;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class ExperienceService : IExperienceService
{
    private readonly IExperiencesRepository _experiencesRepository;

    public ExperienceService(IExperiencesRepository experiencesRepository)
    {
        _experiencesRepository = experiencesRepository;
    }

    public async Task<List<Experiences>> FindAllAsync()
    {
        var experience = await _experiencesRepository.FindAll().ToListAsync();
        if (experience.Count==0)
        {
            throw new NotFoundException("Empty");
        }
        return experience;
    }

    //crude
    public async Task CreateAsync(ExperienceCreateDto experience)
    {
        if (experience == null)
        {
            throw new ArgumentNullException("null argument");
        }

        Experiences exp = new()
        {
            Type = experience.Experience
        };
        await _experiencesRepository.CreateAsync(exp);
        await _experiencesRepository.SaveAsync();
    }

    public async Task Delete(int id)
    {
        var experience = await _experiencesRepository.FindByIdAsync(id);
        if (experience == null)
        {
            throw new NotFoundException("Not found");
        }
        _experiencesRepository.Delete(experience);
        await _experiencesRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, ExperienceUpdateDto experience)
    {
        var baseExperience = await _experiencesRepository.FindByIdAsync(id);
        if (baseExperience == null)
        {
            throw new NotFoundException("Not found");
        }
        if (experience == null)
        {
            throw new ArgumentNullException("Null argument");
        }
        baseExperience.Type = experience.Experience;
        _experiencesRepository.Update(baseExperience);
        await _experiencesRepository.SaveAsync();
    }
}
