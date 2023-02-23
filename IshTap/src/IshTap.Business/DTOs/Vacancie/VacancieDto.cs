using IshTap.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.Vacancie;

public class VacancieDto
{
    public int Id { get; set; }
    public string? Image { get; set; }

    public string? Title { get; set; }

    public string? Address { get; set; }

    public int? Salary { get; set; }

    public DateTime? PublishedOn { get; set; }
    public DateTime? ExpireOn { get; set; }

    public string? JobDesctiption { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }

    public string? Responsibility { get; set; }

    public bool? IsActive { get; set; }

    public int? Views { get; set; }

    public string? Category { get; set; }

    public string? JobType { get; set; }

}
