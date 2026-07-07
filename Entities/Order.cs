using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }
        public Guid BusinessId { get; set; }
        [ForeignKey("BusinessId")]
        public Business Business { get; set; } = null!;

        public StatusEnum StatusId { get; set; }
        public Status Status { get; set; }

        public int OrderNumber { get; set; }

        public ICollection<OrderPackage> OrderPackages { get; set; } = new List<OrderPackage>();
    }
}