using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Entities
{
    public class Package
    {
        public Guid Id { get; set; }

        public Guid BusinessId { get; set; }
        public Business Business { get; set; } = null!;

        public PackageTypeEnum PackageTypeId { get; set; }
        public PackageType PackageType { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime PickupStart { get; set; }
        public DateTime PickupEnd { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<OrderPackage> OrderPackages { get; set; } = new List<OrderPackage>();
    }
}