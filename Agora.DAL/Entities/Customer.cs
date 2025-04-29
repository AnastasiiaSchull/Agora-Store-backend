
namespace Agora.DAL.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<Payment>? Payments { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<GiftCard>? GiftCards { get; set; }
        public virtual ICollection<BankCard>? BankCards { get; set; }
        public virtual Cashback? Cashback { get; set; }
        public virtual ICollection<SellerReview>? SellerReviews { get; set; }
        public virtual ICollection<ProductReview>? ProductReviews { get; set; }
        public virtual ICollection<Wishlist>? Wishlists { get; set; }
        public virtual ICollection<Support>? Supports { get; set; }
        public virtual ICollection<Return>? Returns { get; set; }
    }
}
