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
            var booksToSkip=(pageNumber-1)*pageSize;
            return await _context.Books.Skip(booksToSkip).Take(pageSize).ToListAsync();
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book?> GetBookByIsbn(string isbn)
        {
            return await _context.Books.SingleOrDefaultAsync(b => b.Isbn == isbn);
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