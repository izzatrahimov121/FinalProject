using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.Vacancie;

public class VacancieUpdateDto
{
    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(150, ErrorMessage = "Uzunluq 150 simvolu keçdi")]
    public string? Title { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public string? Address { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public int? Salary { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(700, ErrorMessage = "Uzunluq 700 simvolu keçdi")]
    public string? JobDesctiption { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(700, ErrorMessage = "Uzunluq 700 simvolu keçdi")]
    public string? Responsibility { get; set; }

    [Required(ErrorMessage = "Boş buraxmayın")]
    public string? ContactPhone { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), DataType(DataType.EmailAddress)]
    public string? ContactEmail { get; set; }


    [Required(ErrorMessage ="Boş buraxmayın")]
    public int CategoryId { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public int JobTypeId { get; set; }
}
