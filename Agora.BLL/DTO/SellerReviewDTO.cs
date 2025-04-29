using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class SellerReviewDTO 
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public decimal Rating { get; set; }
        public DateOnly Date { get; set; }
        
        public int? SellerId { get; set; }
        public CustomerDTO? Customer { get; set; }
    }
}
