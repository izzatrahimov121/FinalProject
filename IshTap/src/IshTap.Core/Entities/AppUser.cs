using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IshTap.Core.Entities;

public class AppUser : IdentityUser
{
    [Required(ErrorMessage ="Boş buraxmayın")]
    public string? Fullname { get; set; }

    public bool? IsActive { get; set; } = true;
}
