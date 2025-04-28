
namespace Agora.DAL.Entities
{
    public class GiftCard
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public decimal Balance { get; set; }
        public DateOnly ExpirationDate { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}
