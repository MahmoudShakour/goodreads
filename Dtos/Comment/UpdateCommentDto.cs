using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Comment
{
    public class UpdateCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}