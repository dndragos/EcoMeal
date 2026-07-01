using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

public partial class OrderPackage
{
    public int OrderId { get; set; }

    public int? PackageId { get; set; }

    public int? Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Package? Package { get; set; }
}
