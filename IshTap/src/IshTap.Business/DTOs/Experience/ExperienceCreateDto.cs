using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.Experience;

public class ExperienceCreateDto
{
    [Required,MaxLength(100)]
    public string? Experience { get; set; }
}
