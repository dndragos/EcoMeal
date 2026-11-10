using BlazorApp1.Data;
using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Repositories
{
    public class PackageRepository(EcoMealDbContext context) : IPackageRepository
    {
        public async Task<List<Package>> GetAllAsync()
        {
            return await context.Packages.Include(p => p.Business).ToListAsync();
        }

        public async Task<Package?> GetById(Guid Id)
        {
            return await context.Packages.Include(p => p.Business).FirstOrDefaultAsync(o => o.Id == Id);
        }

        public async Task AddAsync(Package package)
        {
            await context.Packages.AddAsync(package);
        }

        public async Task DeleteAsync(Guid id)
        {
            var package = await context.Packages
                .Include(p => p.OrderPackages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (package is null)
                return;

            context.OrderPackages.RemoveRange(package.OrderPackages);
            context.Packages.Remove(package);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<(int Quantity, Guid BusinessId)?> GetPackageAvailabilityAsync(Guid packageId)
        {
            var package = await context.Packages
                .Where(p => p.Id == packageId)
                .Select(p => new { p.Quantity, p.BusinessId })
                .FirstOrDefaultAsync();

            if (package == null) return null;
            return (package.Quantity, package.BusinessId);
        }

        public async Task UpdatePackageQuantityAsync(Guid packageId, int quantityChange)
        {
            await context.Packages
                .Where(p => p.Id == packageId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.Quantity, p => p.Quantity + quantityChange));
        }
    }
}
