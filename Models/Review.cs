using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Database;

namespace goodreads.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string AppUserId { get; set; } = string.Empty;
        public int BookId { get; set; }
        public List<Comment> Comments { get; set; }=[];
        public List<Like> Likes { get; set; }=[];
    }
}