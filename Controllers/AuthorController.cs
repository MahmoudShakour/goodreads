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
        private readonly TokenInfo? _tokenInfo;

        public AuthorController(IAuthorRepo authorRepo, IJWTHelper jWTHelper)
        {
            _authorRepo = authorRepo;
            _tokenInfo = jWTHelper.DecodeToken();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDto createAuthorDto)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
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

        [HttpGet("page/{pageNumber}")]
        public async Task<IActionResult> GetAll([FromRoute] int pageNumber)
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAuthorDto updateAuthorDto)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
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

        [HttpGet("{id}/books")]
        public async Task<IActionResult> GetBooks([FromRoute] int id)
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
            var booksDto = books.Select(b => b.ToBookDto());
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
    }
}