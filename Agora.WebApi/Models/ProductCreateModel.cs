namespace Agora.Models
{
    public class ProductCreateModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public int? StoreId { get; set; }
        public int? SubcategoryId { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }

        public List<IFormFile>? Images { get; set; } // ключ "Images" из FormData
    }
}
