using BlazorApp1.Data;
using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly EcoMealDbContext _context;

        public ReviewRepository(EcoMealDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<IEnumerable<Review>> GetByPackageIdAsync(Guid packageId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.PackageId == packageId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Reviews.Where(r => r.Id == id).ExecuteDeleteAsync();
        }
    }
}
