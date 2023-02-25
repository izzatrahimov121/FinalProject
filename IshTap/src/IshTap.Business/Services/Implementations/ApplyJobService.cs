using AutoMapper;
using IshTap.Business.DTOs.ApplyJob;
using IshTap.Business.Exceptions;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IshTap.Business.Services.Implementations;

public class ApplyJobService : IApplyJobService
{
    private readonly AppDbContexts _contexts;
    private readonly IVacancieRepository _vacancieRepository;
    private readonly IFileService _fileService;
    private readonly IHostingEnvironment _env;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public ApplyJobService(IVacancieRepository vacancieRepository,
                           IFileService fileService,
                           IHostingEnvironment env,
                           AppDbContexts contexts,
                           UserManager<AppUser> userManager,
                           IMapper mapper)
    {
        _vacancieRepository = vacancieRepository;
        _fileService = fileService;
        _env = env;
        _contexts = contexts;
        _userManager = userManager;
        _mapper = mapper;
    }
    private DbSet<ApplyJob> _table => _contexts.Set<ApplyJob>();
    public async Task Created(int vacancieId, string userId, ApplyJobCreateDto applyJob)
    {
        var vacancie = await _vacancieRepository.FindByIdAsync(vacancieId);
        if (vacancie is null) { throw new NotFoundException("Not found"); }
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) { throw new NotFoundException("User not found"); }
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
            CV = fileName,
            UserId = userId
        };
        await _table.AddAsync(apply);
        await _contexts.SaveChangesAsync();
    }

    public async Task<List<ApplyJobDto>> Applications(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) { throw new NotFoundException("User not found"); }
        var applications = await _table.AsQueryable().AsNoTracking().Where(a=>a.UserId==user.Id).ToListAsync();
        var result = _mapper.Map<List<ApplyJobDto>>(applications);
        return result;
    }

}
