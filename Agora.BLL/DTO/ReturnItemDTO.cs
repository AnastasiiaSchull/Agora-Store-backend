namespace Agora.BLL.DTO
{
    public class ReturnItemDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string? Reason { get; set; }
        public int? ReturnId { get; set; }
        public int? ProductId { get; set; }
        public int? OrderItemId { get; set; }
        public OrderItemDTO? OrderItemDTO { get; set; }
        public ReturnDTO? ReturnDTO { get; set; }

    }
}
