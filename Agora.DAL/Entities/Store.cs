
namespace Agora.DAL.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly? UpdatedAt { get; set; }

        public int? SellerId { get; set; }
        public virtual Seller? Seller { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
