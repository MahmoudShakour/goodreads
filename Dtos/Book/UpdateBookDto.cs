using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Book
{
    public class UpdateBookDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Isbn { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(1600, 2024)]
        public int Year { get; set; }
        [Required]
        public string Genre { get; set; } = string.Empty;
        [Required]
        [Range(0, 2000)]
        public int NumberOfPages { get; set; }
    }
}