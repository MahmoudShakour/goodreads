using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Author
{
    public class UpdateAuthorDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateTime BornAt { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DiedAt { get; set; }
        [Required]
        public string Summary { get; set; } = string.Empty;
    }
}