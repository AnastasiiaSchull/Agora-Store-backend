
namespace Agora.DAL.Entities
{
    public class ProductReview
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public decimal Rating { get; set; }
        public DateOnly Date { get; set; }

        public virtual int? ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public virtual int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
