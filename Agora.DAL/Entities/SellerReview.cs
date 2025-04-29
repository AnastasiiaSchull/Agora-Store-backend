
namespace Agora.DAL.Entities
{
    public class SellerReview
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public decimal Rating { get; set; }
        public DateOnly Date { get; set; }

        public int? SellerId { get; set; }
        public virtual Seller? Seller { get; set; }        
             
        public virtual Customer? Customer { get; set; }
    }
}
