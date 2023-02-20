using IshTap.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IshTap.Business.DTOs.CV;

public class CVDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? FatherName { get; set; }
    public string? Iamge { get; set; }
    public string? AboutYourself { get; set; }
    public string? Position { get; set; }
    public string? City { get; set; }
    public int? MinSalary { get; set; }
    public string? Skills { get; set; }
    public string? Details { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? PublishedOn { get; set; }
    public DateTime? ExpireOn { get; set; }
    public int? Views { get; set; }
    public bool? IsActive { get; set; } 
    public string? Education { get; set; }
    public string? Category { get; set; }
    public string? Experience { get; set; }
}
