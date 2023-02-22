using IshTap.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IshTap.Core.Entities;

public class GetInTouch : IEntity
{
    public int Id { get; set; }

    [Required,MaxLength(1000),NotNull]
    public string? Message { get; set; }

    [Required,MaxLength(50),NotNull]
    public string? Name { get; set; }

    [Required,MaxLength(50),NotNull]
    public string? Subject { get; set; }

    [Required,MaxLength(256),NotNull,DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    public string? UserId { get; set; }
    public AppUser AppUser { get; set; }
}
