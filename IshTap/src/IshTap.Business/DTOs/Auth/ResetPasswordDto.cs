using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IshTap.Business.DTOs.Auth;

public class ResetPasswordDto
{
    [Required,MinLength(8),DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Required, MinLength(8), NotNull, Compare(nameof(NewPassword))]
    public string? ConfirmNewPassword { get; set; }
}
