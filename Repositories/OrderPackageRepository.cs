using BlazorApp1.Data;
using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Repositories
{
    public class OrderPackageRepository(EcoMealDbContext context) : IOrderPackageRepository
    {
        public async Task<List<OrderPackage>> GetAllAsync()
        {
            return await context.OrderPackages
                .Include(op => op.Order)
                .Include(op => op.Package)
                .ToListAsync();
        }

        public async Task<OrderPackage?> GetById(Guid Id)
        {
            return await context.OrderPackages
                .Include(op => op.Order)
                .Include(op => op.Package)
                .FirstOrDefaultAsync(op => op.Id == Id);
        }

        public async Task AddAsync(OrderPackage orderPackage)
        {
            await context.OrderPackages.AddAsync(orderPackage);

        }

        public async Task DeleteAsync(Guid id)
        {
            var orderPackage = await context.OrderPackages.FindAsync(id);
            if (orderPackage != null)
            {
                context.OrderPackages.Remove(orderPackage);
            }
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
