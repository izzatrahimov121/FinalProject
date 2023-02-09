using IshTap.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace IshTap.Core.Entities;

public class JobType : IEntity
{
    public int Id { get; set; }

    [Required,MaxLength(100)]
    public string? Type { get; set; }

    ICollection<Vacancie>? Vacancie { get; set;}
}
