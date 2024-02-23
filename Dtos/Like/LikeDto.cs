using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Like
{
    public class LikeDto
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        public int ReviewId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}