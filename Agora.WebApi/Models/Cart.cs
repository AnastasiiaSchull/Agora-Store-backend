using Agora.DAL.Entities;

namespace Agora.Models
{
    public class Cart
    {
        public int ProductId { get; set; }
        public int DeliveryOptionId { get; set; }
        public int Quantity { get; set; }

        public int DeliveryPrice { get; set; }

    }
}
