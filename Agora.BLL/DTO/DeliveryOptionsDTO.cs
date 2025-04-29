using Agora.Enums;

namespace Agora.BLL.DTO
{
    public class DeliveryOptionsDTO
    {
        public int Id { get; set; }
        public DeliveryType Type { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDays { get; set; }
        public int? ShippingId { get; set; }
    }
}
