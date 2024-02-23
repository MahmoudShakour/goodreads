using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Helpers;
using goodreads.Helpers.Contracts;
using goodreads.Mappers;
using goodreads.Models;
using goodreads.Repos;
using goodreads.Repos.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace goodreads.Controllers
{
    [ApiController]
    [Route("api/like")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepo _likeRepo;
        private readonly IReviewRepo _reviewRepo;
        private readonly IAuthRepo _authRepo;
        private readonly TokenInfo _tokenInfo;

        public LikeController(IReviewRepo reviewRepo, ILikeRepo likeRepo, IAuthRepo authRepo, IHttpContextAccessor httpContextAccessor, IJWTHelper jWTHelper)
        {
            _reviewRepo = reviewRepo;
            _likeRepo = likeRepo;
            _authRepo = authRepo;
            var token = httpContextAccessor?.HttpContext?.Request.Headers.Authorization;
            _tokenInfo = jWTHelper.DecodeToken(token);
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserLikes([FromRoute] string userId)
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
                                message = "user is not found"
                            }
                        );
                }

                var likes = await _likeRepo.GetUserLikes(userId);
                var likesDto = likes.Select(l => l.ToLikeDto());

                return
                    Ok(
                        new
                        {
                            success = true,
                            statusCode = 200,
                            message = "user likes returned successfully",
                            data = likesDto,
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
        public async Task<IActionResult> GetReviewLikes([FromRoute] int reviewId)
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
                                message = "review is not found"
                            }
                        );
                }

                var likes = await _likeRepo.GetReviewLikes(reviewId);
                var likesDto = likes.Select(l => l.ToLikeDto());

                return
                    Ok(
                        new
                        {
                            success = true,
                            statusCode = 200,
                            message = "review likes returned successfully",
                            data = likesDto,
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
        public async Task<IActionResult> Create([FromRoute] int reviewId)
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

                var review = await _reviewRepo.GetById(reviewId);
                if (review == null)
                {
                    return
                       NotFound(
                           new
                           {
                               success = false,
                               statusCode = 404,
                               message = "review is not found"
                           }
                       );
                }

                var like = new Like { AppUserId = user.Id, ReviewId = review.Id };

                var exists = await _likeRepo.Exists(like);
                if (exists)
                {
                    return
                       BadRequest(
                           new
                           {
                               success = false,
                               statusCode = 400,
                               message = "like already exists."
                           }
                       );
                }

                await _likeRepo.Create(like);
                return
                    StatusCode(
                        201,
                        new
                        {
                            success = true,
                            statusCode = 201,
                            message = "like is created successfully"
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

        [HttpDelete("{likeId}")]
        public async Task<IActionResult> Delete([FromRoute] int likeId)
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

                var like = await _likeRepo.GetById(likeId);
                if (like == null)
                {
                    return
                       NotFound(
                           new
                           {
                               success = false,
                               statusCode = 404,
                               message = "like is not found"
                           }
                       );
                }

                if (like.AppUserId != user.Id)
                {
                    return
                       StatusCode(
                            403,
                            new
                            {
                                success = false,
                                statusCode = 403,
                                message = "you don't have access to delete the like"
                            }
                       );
                }

                await _likeRepo.Delete(like);
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