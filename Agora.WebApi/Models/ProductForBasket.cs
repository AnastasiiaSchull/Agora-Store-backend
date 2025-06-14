using Agora.BLL.DTO;

namespace Agora.Models
{
    public class ProductForBasket
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public decimal Rating { get; set; }
        public bool IsAvailable { get; set; }
        public int? StoreId { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public List<DeliveryOptionsDTO> DeliveryOptions { get; set; }
    }
}
