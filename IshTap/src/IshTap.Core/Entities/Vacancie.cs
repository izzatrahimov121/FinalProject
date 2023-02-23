using IshTap.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace IshTap.Core.Entities;

public class Vacancie : IEntity
{
    public int Id { get; set; }

    public string? Image { get; set; }

    [Required(ErrorMessage ="Boş buraxmayın"), MaxLength(150, ErrorMessage = "Uzunluq 150 simvolu keçdi")]
    public string? Title { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public string? Address { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public string? ContactPhone { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"),DataType(DataType.EmailAddress)]
    public string? ContactEmail { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public int? Salary { get; set; }


    public DateTime? PublishedOn {get; set; }

    public DateTime? ExpireOn { get;set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(700, ErrorMessage = "Uzunluq 700 simvolu keçdi")]
    public string? JobDesctiption { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(700, ErrorMessage = "Uzunluq 700 simvolu keçdi")]
    public string? Responsibility { get; set; }

    public int? Views { get; set; } = 0;

    public bool? IsActive { get; set; }

    [Required]
    public string UserId { get; set; }
    public AppUser? AppUser { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Required]
    public int JobTypeId { get; set; }
    public JobType? JobType { get; set; }
}
