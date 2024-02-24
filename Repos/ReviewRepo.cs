using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Database;
using goodreads.Models;
using goodreads.Repos.Contracts;
using Microsoft.EntityFrameworkCore;

namespace goodreads.Repos
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Review> Create(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task Delete(Review review)
        {
            await _context.Likes.Where(l=>l.ReviewId==review.Id).ExecuteDeleteAsync();
            await _context.Comments.Where(c=>c.ReviewId==review.Id).ExecuteDeleteAsync();
            await _context.Comments.Where(c=>c.ReviewId==review.Id).ExecuteDeleteAsync();
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Review>> GetBookReviews(int bookId)
        {
            return await _context.Reviews.Where(r => r.BookId == bookId).ToListAsync();
        }

        public async Task<Review?> GetById(int reviewId)
        {
            return await _context.Reviews.FindAsync(reviewId);
        }

        public async Task<List<Review>> GetUserReviews(string userId)
        {
            return await _context.Reviews.Where(r => r.AppUserId == userId).ToListAsync();
        }

        public async Task<bool> IsDuplicate(string userId, int bookId)
        {
            return await _context.Reviews.AnyAsync(r => r.AppUserId == userId && r.BookId == bookId);
        }

        public async Task<Review?> Update(Review updatedReview)
        {
            var review = await _context.Reviews.FindAsync(updatedReview.Id);
            if (review == null)
                return null;

            review.Content = updatedReview.Content;
            await _context.SaveChangesAsync();

            return review;
        }
    }
}