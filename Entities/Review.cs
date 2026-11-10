using System;

namespace BlazorApp1.Entities
{
    public class Review
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Guid PackageId { get; set; }
        public Package Package { get; set; } = null!;

        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
