using Agora.Enums;

namespace Agora.DAL.Entities
{
    public class Shipping
    {
        public int Id { get; set; }
        public ShippingStatus Status { get; set; }                                            
        public string? TrackingNumber { get; set; }
         
        public virtual Address? Address { get; set; }
        public virtual Seller? Seller{ get; set; }

        public int? OrderItemId { get; set; }
        public virtual OrderItem? OrderItem { get; set; }

        public int? DeliveryOptionsId { get; set; }
        public virtual DeliveryOptions? DeliveryOptions { get; set; }
    }
}
