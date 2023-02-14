using System.ComponentModel.DataAnnotations;

namespace IshTap.Business.DTOs.Auth;

public class ForgotPasswordDto
{
    [Required, MaxLength(256), DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
}
