using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Review
{
    public class UpdateReviewDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }=string.Empty;
    }
}