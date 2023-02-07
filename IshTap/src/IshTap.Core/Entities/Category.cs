﻿using IshTap.Core.Interfaces;

namespace IshTap.Core.Entities;

public class Category : IEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? UsesCount { get; set; }
    ICollection<Vacancie>? Vacancie { get; set;}
}
