
namespace Agora.DAL.Entities
{
    public class ReturnItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string? Reason { get; set; }

        public int? ReturnId { get; set; }
        public virtual Return? Return { get; set; }

        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int? OrderItemId { get; set; }
        public virtual OrderItem? OrderItem { get; set; }
    }
}
