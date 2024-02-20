using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.Author;
using goodreads.Helpers;
using goodreads.Helpers.Contracts;
using goodreads.Mappers;
using goodreads.Repos.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace goodreads.Controllers
{
    [ApiController]
    [Route("/api/author")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepo _authorRepo;
        private readonly TokenInfo _tokenInfo;

        public AuthorController(IAuthorRepo authorRepo, IHttpContextAccessor httpContextAccessor, IJWTHelper jWTHelper)
        {
            _authorRepo = authorRepo;
            var token = httpContextAccessor?.HttpContext?.Request.Headers.Authorization;
            _tokenInfo = jWTHelper.DecodeToken(token);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDto createAuthorDto)
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

                if (_tokenInfo.Role != "Admin")
                {
                    return
                        StatusCode(403,
                            new
                            {
                                success = false,
                                statusCode = 403,
                                message = "you don't have the access to create authors"
                            }
                        );
                }

                await _authorRepo.Create(createAuthorDto.ToAuthor());
                return
                    StatusCode(
                    201,
                    new
                    {
                        success = true,
                        statusCode = 201,
                        message = "author created successfully."
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
                            statusCode = 401,
                            message = "Unauthorized"
                        }
                    );
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
        {
            try
            {
                var author = await _authorRepo.GetById(id);
                if (author == null)
                {
                    return
                        NotFound(
                            new
                            {
                                success = false,
                                statusCode = 404,
                                message = "author is not found"
                            }
                        );
                }

                return
                    Ok(
                        new
                        {
                            success = true,
                            statusCode = 200,
                            message = "author returned successfully.",
                            data = author.ToAuthorDto(),
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
                            statusCode = 401,
                            message = "Unauthorized"
                        }
                    );
            }
        }

        [HttpGet("page/{pageNumber}")]
        public async Task<IActionResult> GetAll([FromRoute] int pageNumber)
        {
            try
            {
                var authors = await _authorRepo.GetAll(pageNumber);
                var authorsDto = authors.Select(a => a.ToAuthorDto());
                return
                    Ok(
                        new
                        {
                            success = true,
                            statusCode = 200,
                            message = "authors returned successfully.",
                            data = authorsDto,
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
                            statusCode = 401,
                            message = "Unauthorized"
                        }
                    );
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAuthorDto updateAuthorDto)
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

                if (_tokenInfo.Role != "Admin")
                {
                    return
                        StatusCode(403,
                            new
                            {
                                success = false,
                                statusCode = 403,
                                message = "you don't have the access to update authors"
                            }
                        );
                }

                var updatedAuthor = await _authorRepo.Update(updateAuthorDto.ToAuthor());
                if (updatedAuthor == null)
                {
                    return
                        NotFound(
                            new
                            {
                                success = false,
                                statusCode = 404,
                                message = "author is not found."
                            }
                        );
                }

                return
                    Ok(
                        new
                        {
                            success = true,
                            statusCode = 200,
                            message = "author is updated successfully.",
                            data = updatedAuthor.ToAuthorDto(),
                        }
                    );
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return StatusCode(
                    500,
                    new
                    {
                        success = false,
                        statusCode = 500,
                        message = "internal server error",
                    }
                );
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
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

                if (_tokenInfo.Role != "Admin")
                {
                    return
                        StatusCode(403,
                            new
                            {
                                success = false,
                                statusCode = 403,
                                message = "you don't have the access to delete authors"
                            }
                        );
                }

                var author = await _authorRepo.GetById(id);
                if (author == null)
                {
                    return
                        NotFound(
                            new
                            {
                                success = false,
                                statusCode = 404,
                                message = "author is not found."
                            }
                        );
                }

                await _authorRepo.Delete(author);
                return NoContent();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return StatusCode(
                    500,
                    new
                    {
                        success = false,
                        statusCode = 500,
                        message = "internal server error",
                    }
                );
            }
        }

        [HttpGet("{id}/books")]
        public async Task<IActionResult> GetBooks([FromRoute] int id)
        {
            try
            {

                var books = await _authorRepo.GetBooks(id);
                if (books == null)
                {
                    return
                        NotFound(
                            new
                            {
                                success = false,
                                statusCode = 404,
                                message = "author is not found"
                            }
                        );
                }
                var booksDto=books.Select(b=>b.ToBookDto());
                return Ok(
                    new
                    {
                        success = true,
                        statusCode = 200,
                        message = "author books returned successfully.",
                        data = booksDto,
                    }
                );
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return StatusCode(
                    500,
                    new
                    {
                        success = false,
                        statusCode = 500,
                        message = "internal server error",
                    }
                );
            }
        }
    }
}