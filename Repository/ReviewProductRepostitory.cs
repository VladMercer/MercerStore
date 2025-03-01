using MercerStore.Data;
using MercerStore.Data.Enum;
using MercerStore.Data.Enum.Review;
using MercerStore.Dtos.ReviewDto;
using MercerStore.Interfaces;
using MercerStore.Models.Products;
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

        public async Task<Review> AddReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task DeleteReview(int? reviewId)
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

        public async Task<int?> GetReviewId(string userId, int productId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId && r.ProductId == productId)
                .Select(r => (int?)r.Id)
                 .FirstOrDefaultAsync();
        }
        public async Task<Review> GetReviewById(int? reviewId)
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

        public async Task<(IEnumerable<AdminReviewDto>, int totalItems)> GetFilteredReviews(
        int pageNumber,
        int pageSize,
        ReviewSortOrder? sortOrder,
        TimePeriod? timePeriod,
        ReviewFilter? filter,
        string? query)
        {

            var currentDay = DateTime.UtcNow;
            var reviewsQuery = _context.Reviews
                .AsNoTracking()
                .Include(r => r.Product)
                .Include(r => r.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                reviewsQuery = reviewsQuery.Where(u =>
                    u.UserId == query ||
                    EF.Functions.ILike(u.User.Email, $"%{query}%") ||
                    EF.Functions.ILike(u.User.UserName, $"%{query}%") ||
                    EF.Functions.ILike(u.Product.Name, $"%{query}%")
                );
            }

            if (timePeriod.HasValue && timePeriod != TimePeriod.All)
            {
                var filterDate = timePeriod switch
                {
                    TimePeriod.Day => currentDay.AddDays(-1),
                    TimePeriod.Week => currentDay.AddDays(-7),
                    TimePeriod.Month => currentDay.AddMonths(-1),
                    TimePeriod.Quarter => currentDay.AddMonths(-3),
                    TimePeriod.Year => currentDay.AddYears(-1),
                    _ => currentDay,
                };

                reviewsQuery = filter switch
                {
                    ReviewFilter.CreateDate => reviewsQuery.Where(o => o.Date >= filterDate),
                    ReviewFilter.EditDate => reviewsQuery.Where(o => o.EditDateTime >= filterDate),
                    _ => reviewsQuery,
                };
            }

            reviewsQuery = filter switch
            {
                ReviewFilter.Value1 => reviewsQuery.Where(u => u.Value == 1),
                ReviewFilter.Value2 => reviewsQuery.Where(u => u.Value == 2),
                ReviewFilter.Value3 => reviewsQuery.Where(u => u.Value == 3),
                ReviewFilter.Value4 => reviewsQuery.Where(u => u.Value == 4),
                ReviewFilter.Value5 => reviewsQuery.Where(u => u.Value == 5),
                ReviewFilter.HasReviewText => reviewsQuery.Where(u => u.ReviewText != ""),
                ReviewFilter.NoReviewText => reviewsQuery.Where(u => u.ReviewText == ""),
                _ => reviewsQuery,
            };

            reviewsQuery = sortOrder switch
            {
                ReviewSortOrder.NameAsc => reviewsQuery.OrderBy(p => p.User.UserName),
                ReviewSortOrder.NameDesc => reviewsQuery.OrderByDescending(p => p.User.UserName),
                ReviewSortOrder.ValueAsc => reviewsQuery.OrderBy(p => p.Value),
                ReviewSortOrder.ValueDesc => reviewsQuery.OrderByDescending(p => p.Value),
                ReviewSortOrder.CreateDateAsc => reviewsQuery.OrderBy(p => p.Date),
                ReviewSortOrder.CreateDateDesc => reviewsQuery.OrderByDescending(p => p.Date),
                ReviewSortOrder.EditDateAsc => reviewsQuery.OrderBy(p => p.EditDateTime),
                ReviewSortOrder.EditDateDesc => reviewsQuery.OrderByDescending(p => p.EditDateTime),
                _ => reviewsQuery.OrderByDescending(p => p.Date)
            };

            var totalItems = await reviewsQuery.CountAsync();

            var users = await reviewsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var reviewDtos = users.Select(u => new AdminReviewDto
            {
                Id = u.Id,
                ProductId = u.ProductId,
                UserId = u.UserId,
                Date = u.Date,
                EditDate = u.EditDateTime,
                productImgUrl = u.Product.MainImageUrl,
                ReviewText = u.ReviewText,
                UserName = u.User.UserName,
                Email = u.User.Email,
                Value = u.Value
            }).ToList();

            return (reviewDtos, totalItems);
        }
        public async Task<object> GetReviewMetric()
        {
            var now = DateTime.UtcNow;
            var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + 1);
            var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var startOfYear = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);


            var reviewsQuery = _context.Reviews
                .Include(r => r.Product)
                .AsNoTracking();
               
            var total = reviewsQuery.Count();
            var averageRating = reviewsQuery.Any() ? reviewsQuery.Average(r => r.Value) : 0;

            var dailyReviews = reviewsQuery.Count(r => r.EditDateTime >= now.Date && r.EditDateTime < now.Date.AddDays(1));
            var weeklyReviews = reviewsQuery.Count(r => r.EditDateTime >= startOfWeek);
            var monthlyReviews = reviewsQuery.Count(r => r.EditDateTime >= startOfMonth);

            var topRatedProducts = await reviewsQuery
                .GroupBy(r => r.Product.Name)
                .Select(g => new { Name = g.Key, Rating = g.Average(x => x.Value) })
                .OrderByDescending(x => x.Rating)
                .Take(5)
                .ToListAsync();

            return new
            {
                Total = total,
                AverageRating = averageRating,
                NewReviews = new
                {
                    Daily = dailyReviews,
                    Weekly = weeklyReviews,
                    Monthly = monthlyReviews
                },
                TopRatedProducts = topRatedProducts
            };
        }
    }
}
