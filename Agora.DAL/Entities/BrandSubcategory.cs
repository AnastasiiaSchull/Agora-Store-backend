namespace Agora.DAL.Entities
{
    public class BrandSubcategory
    {
        public int BrandId { get; set; }
        public int SubcategoryId { get; set; }

        public virtual Brand Brand { get; set; } = null!;
        public virtual Subcategory Subcategory { get; set; } = null!;
    }
}
