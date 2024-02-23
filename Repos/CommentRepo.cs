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
    public class CommentRepo : ICommentRepo
    {
        private readonly ApplicationDbContext _context;

        public CommentRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> Create(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Comment?> FindById(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<List<Comment>> ReviewComments(int reviewId)
        {
            return await _context.Comments.Where(c => c.ReviewId == reviewId).ToListAsync();
        }

        public async Task<Comment?> Update(Comment updatedComment)
        {
            var comment = await _context.Comments.FindAsync(updatedComment.Id);

            if (comment == null)
                return null;

            comment.Content = updatedComment.Content;
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<List<Comment>> UserComments(string userId)
        {
            return await _context.Comments.Where(c => c.AppUserId == userId).ToListAsync();

        }
    }
}