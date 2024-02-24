using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.Review;
using goodreads.Helpers;
using goodreads.Helpers.Contracts;
using goodreads.Mappers;
using goodreads.Repos;
using goodreads.Repos.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace goodreads.Controllers
{
    [ApiController]
    [Route("/api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly TokenInfo? _tokenInfo;
        private readonly IReviewRepo _reviewRepo;
        private readonly IBookRepo _bookRepo;
        private readonly IAuthRepo _authRepo;


        public ReviewController(IAuthRepo authRepo, IBookRepo bookRepo, IReviewRepo reviewRepo, IJWTHelper jWTHelper)
        {
            _authRepo = authRepo;
            _bookRepo = bookRepo;
            _reviewRepo = reviewRepo;
            _tokenInfo = jWTHelper.DecodeToken();
        }

        [HttpPost("book/{bookId}")]
        public async Task<IActionResult> Create([FromRoute] int bookId, [FromBody] CreateReviewDto createReviewDto)
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

            var book = await _bookRepo.GetBookById(bookId);
            if (book == null)
            {
                return
                    NotFound(
                        new
                        {
                            success = false,
                            statusCode = 404,
                            message = "book is not found"
                        }
                    );
            }

            var review = createReviewDto.ToReview(user.Id, bookId);
            if (await _reviewRepo.IsDuplicate(user.Id, bookId))
            {
                return
                    BadRequest(
                        new
                        {
                            success = false,
                            statusCode = 400,
                            message = "user has written a review for this book."
                        }
                    );
            }

            var createdReview = await _reviewRepo.Create(review);
            return
                StatusCode(
                    201,
                    new
                    {
                        success = true,
                        statusCode = 201,
                        message = "review created successfully.",
                        data = createdReview,
                    }
                );
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> Delete([FromRoute] int reviewId)
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

            if (review.AppUserId != user.Id)
            {
                return
                    StatusCode(
                        403,
                        new
                        {
                            success = false,
                            statusCode = 403,
                            message = "not authorized to delete the review"
                        }
                    );
            }

            await _reviewRepo.Delete(review);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateReviewDto updateReviewDto)
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

            var reviewExists = await _reviewRepo.GetById(updateReviewDto.Id);
            if (reviewExists == null)
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

            var review = updateReviewDto.ToReview(reviewExists.AppUserId);
            if (review.AppUserId != user.Id)
            {
                return
                    StatusCode(
                        403,
                        new
                        {
                            success = false,
                            statusCode = 403,
                            message = "not authorized to update the review"
                        }
                    );
            }

            var updatedReview = await _reviewRepo.Update(review);
            if (updatedReview == null)
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
            return
                StatusCode(
                    200,
                    new
                    {
                        success = true,
                        statusCode = 200,
                        message = "review updated successfully.",
                        data = updatedReview,
                    }
                );
        }


        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetOne([FromRoute] int reviewId)
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

            return
                Ok(
                    new
                    {
                        success = true,
                        statusCode = 200,
                        message = "review returned successfully",
                        data = review,
                    }
                );
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetBookReviews([FromRoute] int bookId)
        {
            var book = await _bookRepo.GetBookById(bookId);
            if (book == null)
            {
                return
                    NotFound(
                        new
                        {
                            success = false,
                            code = 404,
                            message = "book is not found",
                        }
                    );
            }
            var reviews = await _reviewRepo.GetBookReviews(bookId);
            return
                Ok(
                    new
                    {
                        success = true,
                        code = 200,
                        message = "book reviews returned successfully",
                        data = reviews,
                    }
                );
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserReviews([FromRoute] string userId)
        {
            var user = await _authRepo.GetUserById(userId);
            if (user == null)
            {
                return
                    NotFound(
                        new
                        {
                            success = false,
                            code = 404,
                            message = "user is not found",
                        }
                    );
            }
            var reviews = await _reviewRepo.GetUserReviews(userId);
            return
                Ok(
                    new
                    {
                        success = true,
                        code = 200,
                        message = "book reviews returned successfully",
                        data = reviews,
                    }
                );
        }

    }
}
