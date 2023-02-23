using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IshTap.Business.DTOs.ApplyJob;

public class ApplyJobDto
{
    [Required, MaxLength(100), NotNull]
    public string Name { get; set; }
    [Required, MaxLength(256), NotNull, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    public IFormFile CV { get; set; }
    public string Website { get; set; }
    [Required, MaxLength(500), NotNull]
    public string Coverletter { get; set; }
}
