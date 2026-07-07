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

        public async Task<Business?> GetById(Guid Id)
        {
            return await context.Businesses.FirstOrDefaultAsync(o => o.Id == Id);
        }

        public async Task AddAsync(Business business)
        {
            await context.Businesses.AddAsync(business);
        }

     
        public async Task DeleteAsync(Guid id)
        {
            var business = await context.Businesses.FindAsync(id);

            if (business is null)
                return;
            context.Businesses.Remove(business);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
