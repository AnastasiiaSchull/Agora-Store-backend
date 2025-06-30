namespace Agora.Models
{
    public class PaymentModel
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public List<Cart> Cart { get; set; }
        

    }
}
