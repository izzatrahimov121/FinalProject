using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IshTap.Core.Entities;

public class AppUser : IdentityUser
{
    [Required(ErrorMessage ="Boş buraxmayın")]
    public string? Fullname { get; set; }
    public string Image { get; set; } = String.Empty;
    ICollection<Vacancie>? Vacancies { get; set; }
    ICollection<CVs>? CVs { get; set; }
    ICollection<GetInTouch>? GetInTouch { get; set;}
}
