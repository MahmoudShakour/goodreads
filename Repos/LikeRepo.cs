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
    public class LikeRepo : ILikeRepo
    {
        private readonly ApplicationDbContext _context;

        public LikeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Like> Create(Like like)
        {
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
            return like;
        }

        public async Task Delete(Like like)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(Like like)
        {
            return await _context.Likes.AnyAsync(l => l.ReviewId == like.ReviewId && l.AppUserId == like.AppUserId);
        }

        public async Task<Like?> GetById(int likeId)
        {
            return await _context.Likes.FindAsync(likeId);
        }

        public async Task<List<Like>> GetReviewLikes(int reviewId)
        {
            return await _context.Likes.Where(l => l.ReviewId == reviewId).ToListAsync();

        }

        public async Task<List<Like>> GetUserLikes(string userId)
        {
            return await _context.Likes.Where(l => l.AppUserId == userId).ToListAsync();
        }
    }
}