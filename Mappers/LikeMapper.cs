using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.Like;
using goodreads.Models;

namespace goodreads.Mappers
{
    public static class LikeMapper
    {
        public static LikeDto ToLikeDto(this Like like)
        {
            return new LikeDto
            {
                Id = like.Id,
                AppUserId = like.AppUserId,
                ReviewId = like.ReviewId,
                CreatedAt = like.CreatedAt,
            };
        }
    }
}