using IshTap.Business.DTOs.ApplyJob;
using IshTap.Business.Exceptions;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class ApplyJobService : IApplyJobService
{
    private readonly AppDbContexts _contexts;
    private readonly IVacancieRepository _vacancieRepository;
    private readonly IFileService _fileService;
    private readonly IHostingEnvironment _env;

    public ApplyJobService(IVacancieRepository vacancieRepository, IFileService fileService, IHostingEnvironment env, AppDbContexts contexts)
    {
        _vacancieRepository = vacancieRepository;
        _fileService = fileService;
        _env = env;
        _contexts = contexts;
    }
    private DbSet<ApplyJob> _table => _contexts.Set<ApplyJob>();
    public async Task Created(int vacancieId, ApplyJobDto applyJob)
    {
        var vacancie = await _vacancieRepository.FindByIdAsync(vacancieId);
        if (vacancie is null) { throw new NotFoundException("Not found"); }
        if (applyJob is null) { throw new ArgumentNullException("null"); }
        string fileName = String.Empty;
        if (applyJob.CV != null)
        {
            fileName = await _fileService.CopyDocumentAsync(applyJob.CV, _env.WebRootPath, "assets", "Document");
        }
        ApplyJob apply = new()
        {
            Name = applyJob.Name,
            Email = applyJob.Email,
            Coverletter = applyJob.Coverletter,
            Website = applyJob.Website,
            CV=fileName
        };
        await _table.AddAsync(apply);
        await _contexts.SaveChangesAsync();
    }

}
