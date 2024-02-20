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
    public class AuthorRepo : IAuthorRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly int pageSize = 10;


        public AuthorRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Author> Create(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task Delete(Author author)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Author>> GetAll(int pageNumber)
        {
            int authorsToSkip = (pageNumber - 1) * pageSize;
            return await _context.Authors.Skip(authorsToSkip).Take(pageSize).ToListAsync();
        }

        public async Task<Author?> GetById(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<Author?> Update(Author updatedAuthor)
        {
            var author = await _context.Authors.FindAsync(updatedAuthor.Id);
            if (author == null)
                return null;

            author.Name = updatedAuthor.Name;
            author.BornAt = updatedAuthor.BornAt;
            author.DiedAt = updatedAuthor.DiedAt;
            author.Summary = updatedAuthor.Summary;
            await _context.SaveChangesAsync();

            return author;
        }
    }
}