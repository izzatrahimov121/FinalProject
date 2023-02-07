using IshTap.Core.Interfaces;

namespace IshTap.Core.Entities;

public class JobType : IEntity
{
    public int Id { get; set; }
    public string? Type { get; set; }

    ICollection<Vacancie>? Vacancie { get; set;}
}
