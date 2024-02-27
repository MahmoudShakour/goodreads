using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Rating
{
    public class UpdateRatingDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Range(0, 5)]
        public int RateValue { get; set; }
    }
}