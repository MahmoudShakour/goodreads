using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;

namespace goodreads.Repos.Contracts
{
    public interface IAuthorRepo
    {
        Task<Author> Create(Author author);
        Task Delete(Author author);
        Task<Author?> GetById(int id);
        Task<List<Author>> GetAll(int pageNumber);
        Task<Author?> Update(Author author);
    }
}