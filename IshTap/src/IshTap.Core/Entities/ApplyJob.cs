using IshTap.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IshTap.Core.Entities;

public class ApplyJob : IEntity
{ 
    public int Id { get; set; }
    [Required,MaxLength(100),NotNull]
    public string Name { get; set; }
    [Required,MaxLength(256),NotNull,DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    public string CV { get; set; }
    public string Website { get; set; }
    [Required,MaxLength(500),NotNull]
    public string Coverletter { get; set; }
}
