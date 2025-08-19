using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.Enums;

namespace Agora.BLL.DTO
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public decimal PriceAtMoment { get; set; }
        public int Quantity { get; set; }
        public string ?FormattedDate { get; set; }

        public ProductDTO ProductDTO { get; set; }
        public int ProductId { get; set; }
        public OrderDTO OrderDTO { get; set; }
        public int OrderId { get; set; }
        public ShippingDTO ShippingDTO { get; set; }
        public int? ShippingId { get; set; } 
        public DateOnly Date { get; set; }
        public string Status { get; set; }

    }
}
