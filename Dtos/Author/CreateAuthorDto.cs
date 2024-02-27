using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace goodreads.Dtos.Author
{
    public class CreateAuthorDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateTime BornAt { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DiedAt { get; set; }
        [Required]
        public string Summary { get; set; } = string.Empty;
    }
}