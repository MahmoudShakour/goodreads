using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.Review;
using goodreads.Models;

namespace goodreads.Mappers
{
    public static class ReviewMapper
    {
        public static Review ToReview(this CreateReviewDto createReviewDto, string userId, int bookId)
        {
            return new Review
            {
                Content = createReviewDto.Content,
                CreatedAt = DateTime.Now,
                AppUserId = userId,
                BookId = bookId,
            };
        }

        public static Review ToReview(this UpdateReviewDto updateReviewDto, string userId)
        {
            return new Review
            {
                Id = updateReviewDto.Id,
                Content = updateReviewDto.Content,
                AppUserId = userId,
            };
        }
    }
}