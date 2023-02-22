using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;

namespace IshTap.DataAccess.Repository.Implementations;

public class GetInTouchRepository : Repository<GetInTouch>, IGetInTouchRepository
{
    public GetInTouchRepository(AppDbContexts contexts) : base(contexts)
    {
    }
}
