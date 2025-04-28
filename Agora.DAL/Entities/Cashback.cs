
namespace Agora.DAL.Entities
{
    public class Cashback
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}
