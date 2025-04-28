
namespace Agora.DAL.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public DateOnly DateAdded { get; set; }
        
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
