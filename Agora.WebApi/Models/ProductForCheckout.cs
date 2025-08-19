using Agora.BLL.DTO;

namespace Agora.Models
{
    public class ProductForCheckout
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
        public string? StoreName { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public DeliveryOptionsDTO DeliveryOption { get; set; }
    }
}
