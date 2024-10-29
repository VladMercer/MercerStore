using MercerStore.Data;
using MercerStore.Interfaces;
using MercerStore.Models;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Repository
{
    public class ReviewProductRepostitory : IReviewProductRepository
    {
        private readonly AppDbContext _context;

        public ReviewProductRepostitory(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReview(int reviewId)
        {
            if (reviewId == null)
            {
                throw new KeyNotFoundException($"Отзыв не найден.");
            }
            var review = await _context.Reviews.FindAsync(reviewId);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Review>> GetAllProductReviews(int productId)
        {
            return await _context.Reviews.Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

        }

        public async Task<IEnumerable<Review>> GetAllReviewByUser(string userId)
        {
            return await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();

        }
        public async Task<IEnumerable<Review>> GetReviewsByProduct(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUser(string userId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> GetReviewId(string userId, int productId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId && r.ProductId == productId)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
        }
        public async Task<Review> GetReviewById(int reviewId)
        {
            return await _context.Reviews.FindAsync(reviewId);
        }
        public async Task UpdateReview(Review review)
        {
            var existingReview = await _context.Reviews.FindAsync(review.Id);
            if (existingReview == null)
            {
                throw new KeyNotFoundException($"Отзыв с ID {review.Id} не найден.");
            }
            _context.Entry(existingReview).CurrentValues.SetValues(review);
            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAvgRateProduct(int productId)
        {
            return Math.Round(await _context.Reviews.Where(r => r.ProductId == productId)
                .Select(r => r.Value)
                .AverageAsync());

        }

        public async Task<int> GetCountProductReviews(int productId)
        {
            return await _context.Reviews.Where(r => r.ProductId == productId).CountAsync();
        }


    }
}
