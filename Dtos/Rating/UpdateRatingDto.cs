using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Rating
{
    public class UpdateRatingDto
    {
        public int Id { get; set; }
        public int RateValue { get; set; }
    }
}