
namespace Agora.BLL.DTO
{
    public class ProductReviewDTO
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public decimal Rating { get; set; }
        public DateOnly? Date { get; set; }

        public int? ProductId { get; set; }
        public virtual ProductDTO? Product { get; set; }

        public int? CustomerId { get; set; }
        public CustomerDTO? Customer { get; set; }
    }
}
