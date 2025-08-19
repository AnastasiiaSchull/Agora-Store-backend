namespace Agora.BLL.DTO
{
    public class BankCardDTO
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string? Holder { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public int? CustomerId { get; set; }
        public int? PaymentMethodId { get; set; }
    }
}
