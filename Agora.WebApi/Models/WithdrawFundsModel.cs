namespace Agora.Models
{
    public class WithdrawFundsModel
    {
        public decimal Amount {  get; set; }
        public string CardNumber { get; set; }
        public int StoreId { get; set; }
    }
}
