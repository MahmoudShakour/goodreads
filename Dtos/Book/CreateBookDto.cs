using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Book
{
    public class CreateBookDto
    {
        public string Isbn { get; set; }=string.Empty;
        public string Title { get; set; }=string.Empty;
        public string Description { get; set; }=string.Empty;
        public int Year { get; set; }
        public string Genre { get; set; }=string.Empty;
        public int NumberOfPages { get; set; }
        public int AuthorId { get; set; }
    }
}