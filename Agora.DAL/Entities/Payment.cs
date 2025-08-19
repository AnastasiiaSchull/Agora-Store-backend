using Agora.Enums;

namespace Agora.DAL.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal CashbackUsed { get; set; }

        public virtual Customer? Customer { get; set; }
        public int CustomerId { get; set; }
        public int? PaymentMethodId { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }

        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }
        public virtual GiftCard? GiftCard { get; set; }

        public int? GiftCardId { get; set; }


        public string? Data { get; set; }
        public string? Signature { get; set; }
    }
}
