namespace Agora.BLL.DTO
{
    public class ReturnItemDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string? Reason { get; set; }
        public int? ReturnId { get; set; }
        public int? ProductId { get; set; }
    }
}
