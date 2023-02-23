using IshTap.Business.DTOs.ApplyJob;

namespace IshTap.Business.Services.Interfaces;

public interface IApplyJobService
{
    Task Created(int vacancieId, ApplyJobDto applyJob);
}
