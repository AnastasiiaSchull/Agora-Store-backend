using Agora.Enums;

namespace Agora.DAL.Entities
{
    public class Discount
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Percentage { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DiscountType Type { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
