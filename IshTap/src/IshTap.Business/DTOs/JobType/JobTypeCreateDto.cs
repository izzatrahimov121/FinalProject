using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.JobType;

public class JobTypeCreateDto
{
    [Required,MaxLength(100)]
    public string? jobType { get; set; }
}
