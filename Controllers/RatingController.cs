using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using goodreads.Dtos.Rating;
using goodreads.Helpers;
using goodreads.Helpers.Contracts;
using goodreads.Mappers;
using goodreads.Repos;
using goodreads.Repos.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace goodreads.Controllers
{
    [ApiController]
    [Route("api/rating")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepo _ratingRepo;
        private readonly IAuthRepo _authRepo;
        private readonly IBookRepo _bookRepo;
        private readonly TokenInfo? _tokenInfo;

        public RatingController(IBookRepo bookRepo, IRatingRepo ratingRepo, IAuthRepo authRepo, IJWTHelper jWTHelper)
        {
            _bookRepo = bookRepo;
            _ratingRepo = ratingRepo;
            _authRepo = authRepo;
            _tokenInfo = jWTHelper.DecodeToken();
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetBookRatings([FromRoute] int bookId)
        {
            var book = await _bookRepo.GetBookById(bookId);
            if (book == null)
            {
                return NotFound(
                    new
                    {
                        success = false,
                        status = 404,
                        message = "book is not found",
                    }
                );
            }

            var ratings = await _ratingRepo.BookRatings(book.Id);
            return Ok(
                new
                {
                    success = true,
                    status = 200,
                    message = "ratings returned successfully",
                    data = ratings,
                }
            );
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRatings([FromRoute] string userId)
        {
            var user = await _authRepo.GetUserById(userId);
            if (user == null)
            {
                return NotFound(
                    new
                    {
                        success = false,
                        status = 404,
                        message = "user is not found",
                    }
                );
            }

            var ratings = await _ratingRepo.UserRatings(userId);
            return Ok(
                new
                {
                    success = true,
                    status = 200,
                    message = "ratings returned successfully",
                    data = ratings,
                }
            );
        }

        [HttpPost("book/{bookId}")]
        public async Task<IActionResult> Create([FromRoute] int bookId, [FromBody] CreateRatingDto createRatingDto)
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
                            message = "book is not found",
                        }
                    );
            }
            var exists = await _ratingRepo.Exists(book.Id, user.Id);
            if (exists)
            {
                return
                    BadRequest(
                        new
                        {
                            success = false,
                            statusCode = 400,
                            message = "rating already exists",
                        }
                    );
            }

            var rating = createRatingDto.ToRating(user.Id, bookId);
            var createdRating = await _ratingRepo.Create(rating);

            return
                StatusCode(
                    201,
                    new
                    {
                        success = true,
                        statusCode = 201,
                        message = "rating added successfully",
                    }
                );
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRatingDto updateRatingDto)
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

            var rating = await _ratingRepo.GetById(updateRatingDto.Id);
            if (rating == null)
            {
                return
                    NotFound(
                        new
                        {
                            success = false,
                            statusCode = 404,
                            message = "rating is not found",
                        }
                    );
            }

            if (rating.AppUserId != user.Id)
            {
                return
                    StatusCode(
                        403,
                        new
                        {
                            success = false,
                            statusCode = 403,
                            message = "you don't have access to update the rating",
                        }
                    );
            }

            var ratingToUpdate = updateRatingDto.ToRating(user.Id);
            var updatedRating = await _ratingRepo.Update(ratingToUpdate);

            if (updatedRating == null)
            {
                return
                    NotFound(
                        new
                        {
                            success = false,
                            statusCode = 404,
                            message = "rating is not found",
                        }
                    );
            }

            return
                NotFound(
                    new
                    {
                        success = true,
                        statusCode = 200,
                        message = "rating updated successfully",
                        data = updatedRating,
                    }
                );
        }

        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> Delete([FromRoute] int ratingId)
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

            var rating = await _ratingRepo.GetById(ratingId);
            if (rating == null)
            {
                return
                    NotFound(
                        new
                        {
                            success = false,
                            statusCode = 404,
                            message = "rating is not found",
                        }
                    );
            }

            if (rating.AppUserId != user.Id)
            {
                return
                    StatusCode(
                        403,
                        new
                        {
                            success = false,
                            statusCode = 403,
                            message = "you don't have access to delete the rating",
                        }
                    );
            }

            await _ratingRepo.Delete(rating);
            return NoContent();
        }
    }
}
