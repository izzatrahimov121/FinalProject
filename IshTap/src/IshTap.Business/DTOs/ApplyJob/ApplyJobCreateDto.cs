using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IshTap.Business.DTOs.ApplyJob;

public class ApplyJobCreateDto
{
    [Required(ErrorMessage = "Bos buraxmayin"), MaxLength(100), NotNull]
    public string Name { get; set; }
    [Required(ErrorMessage = "Bos buraxmayin"), MaxLength(256), NotNull, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required(ErrorMessage = "Bos buraxmayin")]
    public IFormFile CV { get; set; }
    public string? Website { get; set; }
    [Required(ErrorMessage = "Bos buraxmayin"), MaxLength(500), NotNull]
    public string Coverletter { get; set; }
}
