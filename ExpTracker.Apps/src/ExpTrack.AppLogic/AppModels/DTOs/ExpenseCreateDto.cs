using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels.DTOs
{
    public class ExpenseCreateDto
    {
        public long UserId { get; set; }
        public decimal Amount { get; set; }
        public string? ExpDesc { get; set; }
        public DateTime ExpDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public long? CategoryId { get; set; }
    }
}
