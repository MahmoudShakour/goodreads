using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Comment
{
    public class CreateCommentDto
    {
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}