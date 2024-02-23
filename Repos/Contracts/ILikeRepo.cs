using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;

namespace goodreads.Repos.Contracts
{
    public interface ILikeRepo
    {
        Task<Like?> GetById(int likeId);
        Task<Like> Create(Like like);
        Task<bool> Exists(Like like);
        Task Delete(Like like);
        Task<List<Like>> GetUserLikes(string userId);
        Task<List<Like>> GetReviewLikes(int reviewId);
    }
}