using IshTap.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IshTap.Business.DTOs.GetInTouch;

public class GetInTouchDto
{
    [Required, MaxLength(1000), NotNull]
    public string? Message { get; set; }

    [Required, MaxLength(50), NotNull]
    public string? Name { get; set; }

    [Required, MaxLength(50), NotNull]
    public string? Subject { get; set; }

    [Required, MaxLength(256), NotNull, DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
}
