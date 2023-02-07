using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;

namespace IshTap.DataAccess.Repository.Implementations;
public class JobTypeRepository : Repository<JobType>, IJobTypeRepository
{
    public JobTypeRepository(AppDbContexts contexts) : base(contexts)
    {
    }
}
