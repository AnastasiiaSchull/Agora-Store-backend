using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.Enums;

namespace Agora.BLL.DTO
{
    public class PaymentDTO//??
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal CashbackUsed { get; set; }
        //public CustomerDTO Customer { get; set; }
        public int? PaymentMethodId { get; set; }
        //public PaymentMethodDTO PaymentMethod { get; set; }
        public int? OrderId { get; set; }
        //public OrderDTO Order { get; set; }
    }
}
