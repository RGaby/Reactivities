using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string Body { get; set; }
        public required string UserName { get; set; }
        public required string DisplayName { get; set; }
        public required string Image { get; set; }
    }
}