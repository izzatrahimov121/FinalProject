using IshTap.Business.DTOs.ApplyJob;

namespace IshTap.Business.Services.Interfaces;

public interface IApplyJobService
{
    Task Created(int vacancieId, string userId, ApplyJobCreateDto applyJob);
    Task<List<ApplyJobDto>> Applications(string userId);
}
