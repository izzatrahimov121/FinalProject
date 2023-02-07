using IshTap.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.Vacancie;

public class VacancieDto
{
    public int Id { get; set; }
    public string? Image { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(150, ErrorMessage = "Uzunluq 150 simvolu keçdi")]
    public string? Title { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public string? Address { get; set; }



    [Required(ErrorMessage = "Boş buraxmayın")]
    public int? Salary { get; set; }


    public DateTime? PlacamentTime { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(700, ErrorMessage = "Uzunluq 700 simvolu keçdi")]
    public string? JobDesctiption { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(700, ErrorMessage = "Uzunluq 700 simvolu keçdi")]
    public string? Responsibility { get; set; }

    public bool? IsActive { get; set; }

    public int? Review { get; set; }

    public int CategoryId { get; set; }

    public int JobTypeId { get; set; }

}
