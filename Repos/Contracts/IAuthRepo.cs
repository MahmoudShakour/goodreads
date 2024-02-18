using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Database;

namespace goodreads.Repos
{
    public interface IAuthRepo
    {
        Task<bool> IsDuplicate(string email, string username);
        Task<bool> CreateUser(AppUser appUser, string password);
        Task<string?> LoginUser(string username, string password);

    }
}