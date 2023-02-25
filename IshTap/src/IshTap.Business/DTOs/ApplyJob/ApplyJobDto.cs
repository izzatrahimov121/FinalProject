using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IshTap.Business.DTOs.ApplyJob;

public class ApplyJobDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string CV { get; set; }
    public string Website { get; set; }
    public string Coverletter { get; set; }
}
