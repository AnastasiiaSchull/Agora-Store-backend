using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.Enums;

namespace Agora.BLL.DTO
{
    public class ShippingDTO //????
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string? TrackingNumber { get; set; }

        public AddressDTO AddressDTO { get; set; }
        public int? OrderItemId { get; set; }
        public int? AddressId { get; set; }
        //public OrderItemDTO OrderItemDTO { get; set; }
        public int? DeliveryOptionsId { get; set; }
        public DeliveryOptionsDTO DeliveryOptionsDTO { get; set; }

        //public DateOnly ShipDate { get; set; }//might be needed
    }
}
