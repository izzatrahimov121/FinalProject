using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.CV;

public class CVCreatedDto
{
    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(50)]
    public string? Name { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(70)]
    public string? Surname { get; set; }


    [MaxLength(50)]
    public string? FatherName { get; set; }


    public IFormFile? Iamge { get; set; }


    [MaxLength(1000)]
    public string? AboutYourself { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(150)]
    public string? Position { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(100)]
    public string? City { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public int? MinSalary { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(1000)]
    public string? Skills { get; set; }


    [MaxLength(1000)]
    public string? Details { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(256),DataType(DataType.EmailAddress)]
    public string? Email { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(20)]
    public string? Phone { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public int EducationId { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public int CategoryId { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public int ExperienceId { get; set; }
}
