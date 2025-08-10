using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.AppModels.DTOs
{
    public class CategoryReadDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
