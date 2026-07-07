using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlazorApp1.Entities;

namespace BlazorApp1.Data
{
    public class EcoMealDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public EcoMealDbContext(DbContextOptions<EcoMealDbContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<OrderPackage> OrderPackages { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<PackageType> PackageTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrderPackage>().HasOne(o => o.Order).WithMany(u => u.OrderPackages).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrderPackage>().HasOne(o => o.Package).WithMany(u => u.OrderPackages).OnDelete(DeleteBehavior.Restrict);

            var statuses = Enum.GetValues(typeof(StatusEnum))
                .Cast<StatusEnum>()
                .Select(e => new Status { Id = e, Name = e.ToString() });
            builder.Entity<Status>().HasData(statuses);

            var businessTypes = Enum.GetValues(typeof(BusinessTypeEnum))
                .Cast<BusinessTypeEnum>()
                .Select(e => new BusinessType { Id = e, Name = e.ToString() });
            builder.Entity<BusinessType>().HasData(businessTypes);

            var packageTypes = Enum.GetValues(typeof(PackageTypeEnum))
                .Cast<PackageTypeEnum>()
                .Select(e => new PackageType { Id = e, Name = e.ToString() });
            builder.Entity<PackageType>().HasData(packageTypes);

        }
    }
    
}
