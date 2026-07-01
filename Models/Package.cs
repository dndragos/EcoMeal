using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

public partial class Package
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? BusinessId { get; set; }

    public int? PackageTypeId { get; set; }

    public int? Price { get; set; }

    public int? Quantity { get; set; }

    public DateTime? PickupStart { get; set; }

    public DateTime? PickupEnd { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Business? Business { get; set; }

    public virtual ICollection<OrderPackage> OrderPackages { get; set; } = new List<OrderPackage>();

    public virtual PackageType? PackageType { get; set; }
}
