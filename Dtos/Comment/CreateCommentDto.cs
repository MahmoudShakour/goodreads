using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodreads.Dtos.Comment
{
    public class CreateCommentDto
    {
        public string Content { get; set; } = string.Empty;
    }
}