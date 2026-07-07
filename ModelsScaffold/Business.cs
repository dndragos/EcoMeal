using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

public partial class Business
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Address { get; set; }

    public string? ImageUrl { get; set; }

    public int? BusinessTypeId { get; set; }

    public virtual BusinessType? BusinessType { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}
