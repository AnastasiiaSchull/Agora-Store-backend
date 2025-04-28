namespace Agora.BLL.DTO
{
    public class CashbackDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int? CustomerId { get; set; }
        public int? PaymentMethodId { get; set; }
    }
}
