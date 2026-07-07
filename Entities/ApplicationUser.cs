using Microsoft.AspNetCore.Identity;

namespace BlazorApp1.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public required string FullName { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
