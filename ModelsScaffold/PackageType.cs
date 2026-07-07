using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

public partial class PackageType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}
