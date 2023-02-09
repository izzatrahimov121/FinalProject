using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;

namespace IshTap.DataAccess.Repository.Implementations;

public class EducationRepository : Repository<Educations>, IEducationRepository
{
    public EducationRepository(AppDbContexts contexts): base(contexts)
    {
    }
}
