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
    public class RatingRepo : IRatingRepo
    {
        private readonly ApplicationDbContext _context;

        public RatingRepo(ApplicationDbContext context)
        {
            _context=context;
        }
        public async Task<List<Rating>> BookRatings(int bookId)
        {
            return await _context.Ratings.Where(r=>r.BookId==bookId).ToListAsync();
        }

        public async Task<Rating> Create(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task Delete(Rating rating)
        {
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<Rating?> Update(Rating updatedRating)
        {
            var rating=await _context.Ratings.FindAsync(updatedRating.Id);
            if(rating==null)
                return null;
            
            rating.RateValue=updatedRating.RateValue;
            await _context.SaveChangesAsync();
            
            return rating;
        }

        public async Task<List<Rating>> UserRatings(string userId)
        {
            return await _context.Ratings.Where(r=>r.AppUserId==userId).ToListAsync();
        }
    }
}