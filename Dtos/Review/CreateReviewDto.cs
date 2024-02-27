using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Review
{
    public class CreateReviewDto
    {
        [Required]
        public string Content { get; set; }=string.Empty;
    }
}