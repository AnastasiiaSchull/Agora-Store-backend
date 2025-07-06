using Agora.Enums;

namespace Agora.BLL.DTO
{
    public class DiscountDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Percentage { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DiscountType Type { get; set; }
        public bool AllProducts { get; set; } = false;
        public List<ProductDTO>? Products { get; set; }
        public List<CategoryDTO>? Categories { get; set; }
        public List<BrandDTO>? Brands { get; set; }
        public List<SubcategoryDTO>? Subcategories { get; set; }

    }
}
