using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.Comment;
using goodreads.Helpers;
using goodreads.Helpers.Contracts;
using goodreads.Mappers;
using goodreads.Repos;
using goodreads.Repos.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace goodreads.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepo _commentRepo;
        private readonly IAuthRepo _authRepo;
        private readonly IReviewRepo _reviewRepo;
        private readonly TokenInfo _tokenInfo;

        public CommentController(IReviewRepo reviewRepo, ICommentRepo commentRepo, IAuthRepo authRepo, IHttpContextAccessor httpContextAccessor, IJWTHelper jWTHelper)
        {
            _reviewRepo = reviewRepo;
            _commentRepo = commentRepo;
            _authRepo = authRepo;
            var token = httpContextAccessor?.HttpContext?.Request.Headers.Authorization;
            _tokenInfo = jWTHelper.DecodeToken(token);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserComments([FromRoute] string userId)
        {
            try
            {
                var user = await _authRepo.GetUserById(userId);
                if (user == null)
                {
                    return
                        NotFound(
                            new
                            {
                                success = false,
                                statusCode = 404,
                                message = "user is not found",
                            }
                        );
                }

                var comments = await _commentRepo.UserComments(userId);
                var commentsDto = comments.Select(c => c.ToCommentDto());

                return
                    Ok(
                        new
                        {
                            success = true,
                            statusCode = 200,
                            message = "comments returned successfully",
                            data = commentsDto,
                        }
                    );

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return
                    StatusCode(500,
                        new
                        {
                            success = false,
                            statusCode = 500,
                            message = "internal server error"
                        }
                    );
            }
        }

        [HttpGet("review/{reviewId}")]
        public async Task<IActionResult> GetReviewComments([FromRoute] int reviewId)
        {
            try
            {
                var review = await _reviewRepo.GetById(reviewId);
                if (review == null)
                {
                    return
                        NotFound(
                            new
                            {
                                success = false,
                                statusCode = 404,
                                message = "review is not found",
                            }
                        );
                }

                var comments = await _commentRepo.ReviewComments(reviewId);
                var commentsDto = comments.Select(c => c.ToCommentDto());

                return
                    Ok(
                        new
                        {
                            success = true,
                            statusCode = 200,
                            message = "comments returned successfully",
                            data = commentsDto,
                        }
                    );

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return
                    StatusCode(500,
                        new
                        {
                            success = false,
                            statusCode = 500,
                            message = "internal server error"
                        }
                    );
            }
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetOne([FromRoute] int commentId)
        {
            try
            {
                var comment = await _commentRepo.FindById(commentId);
                if (comment == null)
                {
                    return
                        NotFound(
                        new
                        {
                            success = false,
                            statusCode = 404,
                            message = "comment is not found"
                        }
                    );
                }

                return
                        Ok(
                        new
                        {
                            success = true,
                            statusCode = 200,
                            message = "comment returned successfully",
                            data = comment.ToCommentDto(),
                        }
                    );
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return
                    StatusCode(500,
                        new
                        {
                            success = false,
                            statusCode = 500,
                            message = "internal server error"
                        }
                    );
            }
        }

        [HttpPost("review/{reviewId}")]
        public async Task<IActionResult> Create([FromRoute] int reviewId, [FromBody] CreateCommentDto createCommentDto)
        {
            try
            {

                if (_tokenInfo == null)
                {
                    return
                        Unauthorized(
                            new
                            {
                                success = false,
                                statusCode = 401,
                                message = "you need to log in"
                            }
                        );
                }

                var user = await _authRepo.GetUserById(_tokenInfo.Id);
                if (user == null)
                {
                    return
                       Unauthorized(
                           new
                           {
                               success = false,
                               statusCode = 401,
                               message = "token is no longer valid"
                           }
                       );
                }

                var review=await _reviewRepo.GetById(reviewId);
                if(review==null){
                    return
                       Unauthorized(
                           new
                           {
                               success = false,
                               statusCode = 404,
                               message = "review is not found"
                           }
                       );
                }

                var comment = createCommentDto.ToComment(user.Id, reviewId);
                await _commentRepo.Create(comment);
                return Created();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return
                    StatusCode(500,
                        new
                        {
                            success = false,
                            statusCode = 500,
                            message = "internal server error"
                        }
                    );
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCommentDto updateCommentDto)
        {
            try
            {

                if (_tokenInfo == null)
                {
                    return
                        Unauthorized(
                            new
                            {
                                success = false,
                                statusCode = 401,
                                message = "you need to log in"
                            }
                        );
                }

                var user = await _authRepo.GetUserById(_tokenInfo.Id);
                if (user == null)
                {
                    return
                       Unauthorized(
                           new
                           {
                               success = false,
                               statusCode = 401,
                               message = "token is no longer valid"
                           }
                       );
                }
                System.Console.WriteLine(updateCommentDto.Id);
                System.Console.WriteLine(updateCommentDto.Id);
                System.Console.WriteLine(updateCommentDto.Id);
                var comment = await _commentRepo.FindById(updateCommentDto.Id);
                System.Console.WriteLine(comment?.Id);
                if (comment == null)
                {
                    return
                       NotFound(
                           new
                           {
                               success = false,
                               statusCode = 404,
                               message = "comment is not found"
                           }
                       );
                }

                if (comment.AppUserId != user.Id)
                {
                    return
                        StatusCode(
                            403,
                            new
                            {
                                success = false,
                                statusCode = 403,
                                message = "you don't have access to update the comment",
                            }
                       );
                }

                var commentToUpdate = updateCommentDto.ToComment(comment.AppUserId, comment.ReviewId);
                var updatedComment = await _commentRepo.Update(commentToUpdate);

                if (updatedComment == null)
                {
                    return
                       NotFound(
                           new
                           {
                               success = false,
                               statusCode = 404,
                               message = "comment is not found"
                           }
                       );
                }

                return
                    Ok(
                        new
                        {
                            success = true,
                            statusCode = 200,
                            message = "comment updated successfully",
                            data = updatedComment,
                        }
                    );
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return
                    StatusCode(500,
                        new
                        {
                            success = false,
                            statusCode = 500,
                            message = "internal server error"
                        }
                    );
            }
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> Delete([FromRoute] int commentId)
        {
            try
            {

                if (_tokenInfo == null)
                {
                    return
                        Unauthorized(
                            new
                            {
                                success = false,
                                statusCode = 401,
                                message = "you need to log in"
                            }
                        );
                }

                var user = await _authRepo.GetUserById(_tokenInfo.Id);
                if (user == null)
                {
                    return
                       Unauthorized(
                           new
                           {
                               success = false,
                               statusCode = 401,
                               message = "token is no longer valid"
                           }
                       );
                }

                var comment = await _commentRepo.FindById(commentId);
                if (comment == null)
                {
                    return
                       NotFound(
                           new
                           {
                               success = false,
                               statusCode = 404,
                               message = "comment is not found"
                           }
                       );
                }

                if (comment.AppUserId != user.Id)
                {
                    return
                        StatusCode(
                            403,
                            new
                            {
                                success = false,
                                statusCode = 403,
                                message = "you don't have access to delete the comment",
                            }
                       );
                }

                await _commentRepo.Delete(comment);
                return NoContent();

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return
                    StatusCode(500,
                        new
                        {
                            success = false,
                            statusCode = 500,
                            message = "internal server error"
                        }
                    );
            }
        }
    }
}