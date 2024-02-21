using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.Rating;
using goodreads.Models;

namespace goodreads.Mappers
{
    public static class RatingMapper
    {
        public static Rating ToRating(this CreateRatingDto createRatingDto, string userId, int bookId)
        {
            return new Rating
            {
                RateValue = createRatingDto.RateValue,
                BookId = bookId,
                AppUserId = userId,
            };
        }

        public static Rating ToRating(this UpdateRatingDto updateRatingDto, string userId)
        {
            return new Rating
            {
                Id=updateRatingDto.Id,
                RateValue = updateRatingDto.RateValue,
                AppUserId = userId,
            };
        }

        public static RatingDto ToRatingDto(this Rating rating)
        {
            return new RatingDto{
                Id=rating.Id,
                AppUserId=rating.AppUserId,
                BookId=rating.BookId,
                RateValue=rating.RateValue,
            };
        }
    }
}