using BlazorApp1.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp1.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> GetByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetByPackageIdAsync(Guid packageId);
        Task AddAsync(Review review);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
