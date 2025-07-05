namespace Agora.DAL.Entities
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public int? DiscountId { get; set; }
        public virtual Discount? Discount { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<BrandSubcategory>? BrandSubcategories { get; set; }
        
    }
}
