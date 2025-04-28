namespace Agora.BLL.DTO
{
    public class GiftCardDTO
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public decimal Balance { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public int? CustomerId { get; set; }
        public int? PaymentMethodId { get; set; }
    }
}
