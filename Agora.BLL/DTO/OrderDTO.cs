using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.Enums;

namespace Agora.BLL.DTO
{
    public class OrderDTO //??
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
   
        public DateOnly PaymentDeadline { get; set; }
       
        //public ProductDTO Product { get; set; }
        public CustomerDTO CustomerDTO { get; set; }
        //public PaymentDTO Payment { get; set; }
        public ShippingDTO ShippingDTO { get; set; }
        //public ReturnDTO Return { get; set; }

    }
}
