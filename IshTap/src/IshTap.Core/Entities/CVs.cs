using IshTap.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace IshTap.Core.Entities;

public class CVs:IEntity
{
    public int Id { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(50)]
    public string? Name { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın"), MaxLength(70)]
    public string? Surname { get; set; }


    [MaxLength(50)]
    public string? FatherName { get; set; }


    public string? Iamge { get; set; }


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

    public DateTime? PublishedOn { get; set; }
    public DateTime? ExpireOn { get; set; }
    public int? Views { get; set; } = 0;
    public bool? IsActive { get; set; } = true;



    [Required(ErrorMessage = "Boş buraxmayın")]
    public int EducationId { get; set; }
    public Educations? Education { get; set; }


    [Required]
    public string UserId { get; set; }
    public AppUser? User { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }


    [Required(ErrorMessage = "Boş buraxmayın")]
    public int ExperienceId { get; set; }
    public Experiences? Experience { get; set; }

}
