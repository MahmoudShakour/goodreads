using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;

namespace goodreads.Repos.Contracts
{
    public interface IRatingRepo
    {
        Task<Rating> Create(Rating rating);
        Task Delete(Rating rating);
        Task<Rating?> Update(Rating rating);
        Task<List<Rating>> UserRatings(string userId);
        Task<List<Rating>> BookRatings(int bookId);

    }
}