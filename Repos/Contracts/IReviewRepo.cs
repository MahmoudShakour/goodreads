using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;

namespace goodreads.Repos.Contracts
{
    public interface IReviewRepo
    {
        Task<Review> Create(Review review);

        Task<Review?> GetById(int reviewId);
        Task<List<Review>> GetBookReviews(int bookId);
        Task<List<Review>> GetUserReviews(string userId);
        Task<Review?> Update(Review updatedReview);
        Task Delete(Review review);

        Task<bool> IsDuplicate(string userId,int bookId);
    }
}