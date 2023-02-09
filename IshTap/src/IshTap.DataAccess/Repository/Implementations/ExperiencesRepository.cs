using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;

namespace IshTap.DataAccess.Repository.Implementations;

public class ExperiencesRepository : Repository<Experiences>, IExperiencesRepository
{
    public ExperiencesRepository(AppDbContexts contexts) : base(contexts)
    {
    }
}
