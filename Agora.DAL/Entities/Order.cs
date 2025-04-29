using Agora.Enums;

namespace Agora.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
      
        public DateOnly PaymentDeadline { get; set; }
       
        public virtual Customer? Customer { get; set; }
        public virtual Payment? Payment { get; set; }                 
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Return>? Returns { get; set; }
    }
}
