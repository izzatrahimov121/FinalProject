using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;

namespace IshTap.DataAccess.Repository.Implementations;

public class CVRepository : Repository<CVs>, ICVRepository
{
    public CVRepository(AppDbContexts contexts) : base(contexts)
    {
    }
}
