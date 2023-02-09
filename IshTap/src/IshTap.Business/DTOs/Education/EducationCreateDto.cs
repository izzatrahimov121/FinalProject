using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.Education;

public class EducationCreateDto
{
    [Required, MaxLength(100)]
    public string? EducationType { get; set; }
}
