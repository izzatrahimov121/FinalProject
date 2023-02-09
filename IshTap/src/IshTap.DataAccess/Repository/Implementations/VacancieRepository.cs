using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;

namespace IshTap.DataAccess.Repository.Implementations;

public class VacancieRepository : Repository<Vacancie>, IVacancieRepository
{
    public VacancieRepository(AppDbContexts contexts) : base(contexts)
    {
    }

}
