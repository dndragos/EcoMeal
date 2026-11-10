using BlazorApp1.Data;
using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Repositories
{
    public class BusinessRepository(EcoMealDbContext context) : IBusinessRepository
    {
        public async Task<List<Business>> GetAllAsync()
        {
            return await context.Businesses.ToListAsync();
        }

        public async Task<List<Business>> GetPaginatedAsync(string? searchTerm, BusinessTypeEnum type, int skip, int take)
        {
            var query = context.Businesses.Where(b => b.BusinessTypeId == type);
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => EF.Functions.Like(b.Name, $"%{searchTerm}%"));
            }
            
            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<Business?> GetById(Guid Id)
        {
            return await context.Businesses.FirstOrDefaultAsync(o => o.Id == Id);
        }

        public async Task<List<Business>> GetByOwnerIdAsync(Guid ownerId)
        {
            return await context.Businesses
                .Include(b => b.BusinessType)
                .Where(b => b.OwnerId == ownerId)
                .ToListAsync();
        }

        public async Task AddAsync(Business business)
        {
            await context.Businesses.AddAsync(business);
        }

     
        public async Task DeleteAsync(Guid id)
        {
            var business = await context.Businesses
                .Include(b => b.Packages).ThenInclude(p => p.OrderPackages)
                .Include(b => b.Orders).ThenInclude(o => o.OrderPackages)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (business is null)
                return;

            var orderPackagesFromPackages = business.Packages.SelectMany(p => p.OrderPackages).ToList();
            var orderPackagesFromOrders = business.Orders.SelectMany(o => o.OrderPackages).ToList();
            var allOrderPackages = orderPackagesFromPackages.Concat(orderPackagesFromOrders).Distinct().ToList();

            context.OrderPackages.RemoveRange(allOrderPackages);
            context.Packages.RemoveRange(business.Packages);
            context.Orders.RemoveRange(business.Orders);
            context.Businesses.Remove(business);
            await context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
