using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public DateTime BornAt { get; set; }
        public DateTime? DiedAt { get; set; }
        public string Summary { get; set; }=string.Empty;
        public List<Book> Books { get; set; }=[];
    }
}