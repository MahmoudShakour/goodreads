using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;

namespace goodreads.Repos.Contracts
{
    public interface ICommentRepo
    {
        Task<Comment?> FindById(int id);
        Task<List<Comment>> ReviewComments(int reviewId);
        Task<List<Comment>> UserComments(string userId);
        Task<Comment> Create(Comment comment);
        Task<Comment?> Update(Comment comment);
        Task Delete(Comment comment);
    }
}