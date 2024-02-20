using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string AppUserId { get; set; } = string.Empty;
        public int BookId { get; set; }
    }
}