
namespace Agora.DAL.Entities
{
    public class BankCard
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string? Holder { get; set; }
        public DateOnly ExpirationDate { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}
