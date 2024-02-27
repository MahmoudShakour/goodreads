using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Database;
using goodreads.Models;
using goodreads.Repos.Contracts;
using Microsoft.EntityFrameworkCore;

namespace goodreads.Repos
{
    public class BookRepo : IBookRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly int pageSize = 10;

        public BookRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Book> CreateBook(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task Delete(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetAll(int pageNumber)
        {
            var booksToSkip = (pageNumber - 1) * pageSize;
            var books = await _context.Books.OrderBy(b => b.Id).Skip(booksToSkip).Take(pageSize).ToListAsync();

            foreach (var book in books)
            {
                var bookRatings = await _context.Ratings.Where(r => r.BookId == book.Id).ToListAsync();
                if (bookRatings.Count != 0)
                    book.Rating = bookRatings.Average(r => r.RateValue);
            }

            return books;
        }

        public async Task<Book?> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return null;

            var rating = await _context.Ratings.Where(r => r.BookId == id).ToListAsync();

            if (rating.Count != 0)
                book.Rating = rating.Average(r => r.RateValue);

            return book;
        }

        public async Task<Book?> GetBookByIsbn(string isbn)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b=>b.Isbn==isbn);
            if (book == null)
                return null;

            var rating = await _context.Ratings.Where(r => r.BookId == book.Id).ToListAsync();
            if (rating.Count != 0)
                book.Rating = rating.Average(r => r.RateValue);

            return book;
        }

        public async Task<List<Book>?> GetBooksByAuthor(int id)
        {
            var books = await _context.Books.Where(b => b.AuthorId == id).ToListAsync();

            foreach (var book in books)
            {
                var bookRatings = await _context.Ratings.Where(r => r.BookId == book.Id).ToListAsync();
                if (bookRatings.Count != 0)
                    book.Rating = bookRatings.Average(r => r.RateValue);
            }

            return books;
        }

        public async Task<Book?> Update(Book UpdatedBook)
        {
            var book = await GetBookById(UpdatedBook.Id);
            if (book == null)
                return null;


            book.Title = UpdatedBook.Title;
            book.Description = UpdatedBook.Description;
            book.Genre = UpdatedBook.Genre;
            book.Isbn = UpdatedBook.Isbn;
            book.NumberOfPages = UpdatedBook.NumberOfPages;

            await _context.SaveChangesAsync();
            return book;
        }
    }
}