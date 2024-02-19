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
                Rating = createBookDto.Rating,
                Genre = createBookDto.Genre,
                NumberOfPages = createBookDto.NumberOfPages,
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
                Rating = updateBookDto.Rating,
                Genre = updateBookDto.Genre,
                NumberOfPages = updateBookDto.NumberOfPages,
            };
        }
    }
}