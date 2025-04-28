
namespace Agora.DAL.Entities
{
    public class Seller
    {
        public int Id { get; set; }
        public float Rating { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<Store>? Stores { get; set; }
        public virtual ICollection<SellerReview>? SellerReviews { get; set; }
        public virtual ICollection<Shipping>? Shippings { get; set; }
        public virtual ICollection<DeliveryOptions>? DeliveryOptions { get; set; }
    }
}
