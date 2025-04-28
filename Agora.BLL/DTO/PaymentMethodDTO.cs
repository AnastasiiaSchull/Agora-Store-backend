using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Agora.BLL.DTO
{
    public class PaymentMethodDTO//??
    {
        public int Id { get; set; }
        public int? BankCardId { get; set; }
        //public BankCardDTO BankCard { get; set; }
        public int? GiftCardId { get; set; }
        //public GiftCardDTO GiftCard { get; set; }
        public int? CashbackId { get; set; }
        //public CashbackDTO Cashback { get; set; }
    }
}
