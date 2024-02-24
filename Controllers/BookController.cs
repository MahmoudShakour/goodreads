using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.Book;
using goodreads.Helpers;
using goodreads.Helpers.Contracts;
using goodreads.Mappers;
using goodreads.Migrations;
using goodreads.Repos.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace goodreads.Controllers
{
    [ApiController]
    [Route("/api/book")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly TokenInfo? _tokenInfo;

        public BookController(IAuthorRepo authorRepo, IBookRepo bookRepo, IJWTHelper jWTHelper)
        {
            _authorRepo = authorRepo;
            _bookRepo = bookRepo;
            _tokenInfo = jWTHelper.DecodeToken();
        }

        [HttpGet("page/{page}")]
        public async Task<IActionResult> GetAll([FromRoute] int page)
        {
            try
            {
                var books = await _bookRepo.GetAll(page);
                var booksDto = books.Select(b => b.ToBookDto());
                return Ok(
                    new
                    {
                        success = true,
                        statusCode = 200,
                        message = "books returned successfully.",
                        data = booksDto
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
        {
            try
            {
                var book = await _bookRepo.GetBookById(id);
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

                return Ok(
                    new
                    {
                        success = true,
                        statusCode = 200,
                        message = "book returned successfully.",
                        data = book.ToBookDto(),
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookDto createBookDto)
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
                                message = "you don't have the access to create books"
                            }
                        );
                }
                var author = await _authorRepo.GetById(createBookDto.AuthorId);
                if (author == null)
                {
                    return
                        NotFound(
                            new
                            {
                                success = false,
                                statusCode = 404,
                                message = "author id is not found",
                            }
                        );
                }

                var book = await _bookRepo.CreateBook(createBookDto.ToBook());
                return
                    StatusCode(
                        201,
                        new
                        {
                            success = true,
                            statusCode = 201,
                            message = "book is created successfully",
                            data = book.ToBookDto(),
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
        public async Task<IActionResult> Update([FromBody] UpdateBookDto updateBookDto)
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
                                message = "you don't have the access to update books"
                            }
                        );
                }

                var book = await _bookRepo.Update(updateBookDto.ToBook());
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

                return Ok(
                    new
                    {
                        success = true,
                        statusCode = 200,
                        message = "book is updated successfully",
                        data = book.ToBookDto(),
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
                System.Console.WriteLine(_tokenInfo.Role);
                System.Console.WriteLine(_tokenInfo.Role);
                System.Console.WriteLine(_tokenInfo.Role);
                if (_tokenInfo.Role != "Admin")
                {
                    return
                        StatusCode(403,
                            new
                            {
                                success = false,
                                statusCode = 403,
                                message = "you don't have the access to delete books"
                            }
                        );
                }

                var book = await _bookRepo.GetBookById(id);
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

                await _bookRepo.Delete(book);
                return
                    StatusCode(204,
                        new
                        {
                            success = true,
                            statusCode = 204,
                            message = "book is deleted successfully"
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

    }
}