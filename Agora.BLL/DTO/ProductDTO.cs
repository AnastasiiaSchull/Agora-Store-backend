using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public decimal Rating { get; set; }
        public string? ImagesPath { get; set; }
        public string? ImagePath { get; set; } //not sure but still
        public bool IsAvailable { get; set; }
        public int? StoreId { get; set; }
        public int? SubcategoryId { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public List<string>? ImagesUrls { get; set; }
    }
}
