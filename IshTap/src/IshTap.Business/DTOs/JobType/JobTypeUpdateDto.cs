using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.JobType;

public class JobTypeUpdateDto
{
    [Required,MaxLength(100)]
    public string? jobType { get; set; }
}
