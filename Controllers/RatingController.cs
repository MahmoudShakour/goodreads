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
        private readonly TokenInfo _tokenInfo;

        public RatingController(IBookRepo bookRepo, IRatingRepo ratingRepo, IAuthRepo authRepo, IHttpContextAccessor httpContextAccessor, IJWTHelper jWTHelper)
        {
            _bookRepo = bookRepo;
            _ratingRepo = ratingRepo;
            _authRepo = authRepo;
            var token = httpContextAccessor?.HttpContext?.Request.Headers.Authorization;
            _tokenInfo = jWTHelper.DecodeToken(token);
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetBookRatings([FromRoute] int bookId)
        {
            try
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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRatings([FromRoute] string userId)
        {
            try
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

        [HttpPost("book/{bookId}")]
        public async Task<IActionResult> Create([FromRoute] int bookId, [FromBody] CreateRatingDto createRatingDto)
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
        public async Task<IActionResult> Update([FromBody] UpdateRatingDto updateRatingDto)
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

        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> Delete([FromRoute] int ratingId)
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



// eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InN0cmluZyIsIlVzZXJJZCI6IjM5NDdlZWUxLWFlNWItNGI3ZC05MGJiLWJmNWFlMjUzNzBhYiIsIlJvbGVOYW1lIjoiVXNlciIsIm5iZiI6MTcwODU1Njc2MywiZXhwIjoxNzExMTQ4NzYzLCJpYXQiOjE3MDg1NTY3NjMsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTI0NiIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTI0NiJ9.tAUGHMNccg4nxI4J-S6pp-m6dlhGR69j3W8IiiswqNM
// eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InN0cmluZzIiLCJVc2VySWQiOiI2YzZhODJkYS1kMDMxLTRhZGMtYmQzOS1iZGNkZjJkNmE4ZjciLCJSb2xlTmFtZSI6IlVzZXIiLCJuYmYiOjE3MDg1NTY3ODgsImV4cCI6MTcxMTE0ODc4OCwiaWF0IjoxNzA4NTU2Nzg4LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUyNDYiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUyNDYifQ.69rAdKFyc8Jbxe8dNK7HcBGMbytsvHw3H_qZ4WoR6vs