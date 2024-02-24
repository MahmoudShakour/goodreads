using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Dtos.Book;
using goodreads.Models;

namespace goodreads.Mappers
{
    public static class BookMapper
    {
        public static Book ToBook(this CreateBookDto createBookDto)
        {
            return new Book
            {
                Isbn = createBookDto.Isbn,
                Title = createBookDto.Title,
                Description = createBookDto.Description,
                Year = createBookDto.Year,
                Genre = createBookDto.Genre,
                NumberOfPages = createBookDto.NumberOfPages,
                AuthorId = createBookDto.AuthorId,
            };
        }

        public static Book ToBook(this UpdateBookDto updateBookDto)
        {
            return new Book
            {
                Id = updateBookDto.Id,
                Isbn = updateBookDto.Isbn,
                Title = updateBookDto.Title,
                Description = updateBookDto.Description,
                Year = updateBookDto.Year,
                Genre = updateBookDto.Genre,
                NumberOfPages = updateBookDto.NumberOfPages,
            };
        }

        public static BookDto ToBookDto(this Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Isbn = book.Isbn,
                Title = book.Title,
                Description = book.Description,
                Year = book.Year,
                Rating = book.Rating,
                Genre = book.Genre,
                NumberOfPages = book.NumberOfPages,
                AuthorId = book.AuthorId,
            };
        }
    }
}