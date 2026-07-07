using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? BusinessId { get; set; }

    public int? StatusId { get; set; }

    public int? OrderNumber { get; set; }

    public virtual Business? Business { get; set; }

    public virtual OrderPackage? OrderPackage { get; set; }

    public virtual Status? Status { get; set; }

    public virtual User? User { get; set; }
}
