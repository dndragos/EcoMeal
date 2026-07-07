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
            return await context.Packages.ToListAsync();
        }

        public async Task<Package?> GetById(Guid Id)
        {
            return await context.Packages.FirstOrDefaultAsync(o => o.Id == Id);
        }

        public async Task AddAsync(Package package)
        {
            await context.Packages.AddAsync(package);
        }

        public async Task DeleteAsync(Guid id)
        {
            var package = await context.Packages.FindAsync(id);

            if (package is null)
                return;
            context.Packages.Remove(package);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
