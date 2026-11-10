using BlazorApp1.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp1.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsForPackageAsync(Guid packageId);
        Task<string?> AddReviewAsync(Guid userId, Guid packageId, int rating, string comment);
        Task DeleteReviewAsync(Guid reviewId);
        Task<double> GetAverageRatingAsync(Guid packageId);
    }
}
