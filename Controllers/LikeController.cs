using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using goodreads.Helpers;
using goodreads.Helpers.Contracts;
using goodreads.Mappers;
using goodreads.Models;
using goodreads.Repos;
using goodreads.Repos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace goodreads.Controllers
{
    [ApiController]
    [Route("api/like")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepo _likeRepo;
        private readonly IReviewRepo _reviewRepo;
        private readonly IAuthRepo _authRepo;
        private readonly TokenInfo? _tokenInfo;

        public LikeController(IReviewRepo reviewRepo, ILikeRepo likeRepo, IAuthRepo authRepo, IJWTHelper jWTHelper)
        {
            _reviewRepo = reviewRepo;
            _likeRepo = likeRepo;
            _authRepo = authRepo;
            _tokenInfo = jWTHelper.DecodeToken();
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserLikes([FromRoute] string userId)
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


        [HttpGet("review/{reviewId}")]
        public async Task<IActionResult> GetReviewLikes([FromRoute] int reviewId)
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

        [HttpPost("review/{reviewId}")]
        public async Task<IActionResult> Create([FromRoute] int reviewId)
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

        [HttpDelete("{likeId}")]
        public async Task<IActionResult> Delete([FromRoute] int likeId)
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

    }
}