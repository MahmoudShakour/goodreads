using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Rating
{
    public class RatingDto
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        public int BookId { get; set; }
        public int RateValue { get; set; }
    }
}