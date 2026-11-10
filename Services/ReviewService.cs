using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using BlazorApp1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<string?> AddReviewAsync(Guid userId, Guid packageId, int rating, string comment)
        {
            if (rating < 1 || rating > 5) return "Rating must be between 1 and 5.";

            var review = new Review
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PackageId = packageId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _reviewRepository.AddAsync(review);
                await _reviewRepository.SaveChangesAsync();
                return null;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return "Database error: Cannot submit review. Your session may be invalid. Please log out and log back in.";
            }
        }

        public async Task<double> GetAverageRatingAsync(Guid packageId)
        {
            var reviews = await _reviewRepository.GetByPackageIdAsync(packageId);
            if (!reviews.Any()) return 0;
            return reviews.Average(r => r.Rating);
        }

        public async Task<IEnumerable<Review>> GetReviewsForPackageAsync(Guid packageId)
        {
            return await _reviewRepository.GetByPackageIdAsync(packageId);
        }

        public async Task DeleteReviewAsync(Guid reviewId)
        {
            await _reviewRepository.DeleteAsync(reviewId);
        }
    }
}
