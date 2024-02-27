using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Rating
{
    public class CreateRatingDto
    {
        [Required]
        [Range(0,5)]
        public int RateValue { get; set; }
    }
}