using IshTap.Core.Interfaces;

namespace IshTap.Core.Entities;

public class FavoriteVacancies : IEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string VacancieId { get; set; }
}
