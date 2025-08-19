
namespace Agora.DAL.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateOnly DateAdded { get; set; }

        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<ProductWishlist>? ProductWishlists { get; set; }
    }
}
