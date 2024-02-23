using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.Comment;
using goodreads.Models;

namespace goodreads.Mappers
{
    public static class CommentMapper
    {
        public static Comment ToComment(this CreateCommentDto createCommentDto, string userId, int reviewId)
        {
            return new Comment
            {
                Content = createCommentDto.Content,
                CreatedAt = DateTime.Now,
                AppUserId = userId,
                ReviewId = reviewId,
            };
        }

        public static Comment ToComment(this UpdateCommentDto updateCommentDto, string userId, int reviewId)
        {
            return new Comment
            {
                Id=updateCommentDto.Id,
                Content = updateCommentDto.Content,
                AppUserId = userId,
                ReviewId = reviewId,
            };
        }

        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                AppUserId = comment.AppUserId,
                ReviewId = comment.ReviewId,
            };
        }
    }
}