using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IshTap.Business.DTOs.Auth;

public class RegisterDto
{
    [Required, MaxLength(256), MinLength(2)]
    public string? Fullname { get; set; }
    [Required, MaxLength(256), MinLength(3), NotNull]
    public string? Username { get; set; }
    [Required, MaxLength(256), NotNull, DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required, MinLength(8), NotNull, DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required,MinLength(8),NotNull,Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }
}
