using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.Experience;

public class ExperienceUpdateDto
{
    [Required,MaxLength(100)]
    public string? Experience { get; set; }
}
