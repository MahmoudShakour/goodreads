using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;

namespace goodreads.Repos.Contracts
{
    public interface IBookRepo
    {
        Task<Book?> GetBookById(int id);
        Task<List<Book>> GetAll(int page);
        Task<Book> CreateBook(Book book);
        Task<Book?> Update(Book book);
        Task Delete(Book book);
    }
}