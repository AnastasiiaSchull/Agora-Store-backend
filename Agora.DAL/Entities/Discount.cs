using Agora.Enums;
using System.ComponentModel.DataAnnotations.Schema;

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
        public bool AllProducts { get; set; } = false;
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<Brand>? Brands { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
        public virtual ICollection<Subcategory>? Subcategories { get; set; }

    }
}
