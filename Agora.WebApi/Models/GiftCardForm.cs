namespace Agora.Models
{
    public class GiftCardForm
    {
        public decimal Balance { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public int CustomerId { get; set; }
    }
}
