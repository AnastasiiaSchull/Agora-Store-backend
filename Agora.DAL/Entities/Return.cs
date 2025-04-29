using Agora.Enums;

namespace Agora.DAL.Entities
{
    public class Return
    {
        public int Id { get; set; }
        public DateOnly ReturnDate { get; set; }
        public ReturnStatus Status{ get; set; } 
        public decimal RefundAmount { get; set; }        

        public virtual Order? Order { get; set; }           
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<ReturnItem>? ReturnItems{ get; set; }
    }
}
