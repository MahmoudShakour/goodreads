using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Review
{
    public class UpdateReviewDto
    {
        public int Id { get; set; }
        public string Content { get; set; }=string.Empty;
    }
}