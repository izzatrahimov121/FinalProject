using IshTap.Business.DTOs.CV;
using IshTap.Business.DTOs.Vacancie;
using IshTap.Core.Entities;
using IshTap.DataAccess.Repository.Interfaces;

namespace IshTap.Business.Services.Interfaces;

public interface ICVService
{
    Task CreateAsync(string userId, CVCreatedDto cv);
    Task UpdateAsync(int id, CVUpdateDto cv);
    Task Delete(int id);
    Task<List<CVDto>> FindAllAsync(int skipt, int take);
    Task<CVDto?> FindByIdAsync(int id);
    Task<List<CVDto>> LastCVsAsync(int count);
}
