using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IshTap.Business.DTOs.Auth;

public class LoginDto
{
    [Required,NotNull]
    public string? UsernameOrEmail { get; set; }
    [Required,MinLength(8),NotNull]
    public string? Password { get; set; }
}
