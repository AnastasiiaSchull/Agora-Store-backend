
namespace Agora.DAL.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? DiscountId { get; set; }
        public virtual Discount? Discount { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<BrandSubcategory>? BrandSubcategories { get; set; }

    }
}
