using Agora.Enums;

namespace Agora.DAL.Entities
{
    public class DeliveryOptions
    {
        public int Id { get; set; }
        public DeliveryType Type { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDays { get; set; }

        public int SellerId { get; set; }
        public virtual ICollection<Shipping>? Shipping { get; set; }
        public virtual Seller? Seller { get; set; }
    }
}
