using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Agora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IUtilsService _utilsService;

        public ProductController(IProductService productService, IUtilsService utilsService)
        {
            _productService = productService;
            _utilsService = utilsService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAll();
            foreach (var item in products)
                item.ImagePath = _utilsService.GetFirstImageUrl(item.ImagesPath, Request);
            return Ok(products);
        }

        [HttpGet("get-all-by-seller/{sellerId}")]
        public async Task<IActionResult> GetAllProductsBySeller(int sellerId )
        {
            var products = await _productService.GetProductsBySeller(sellerId);
            foreach(var item in products)
                item.ImagePath = _utilsService.GetFirstImageUrl(item.ImagesPath, Request);
            if (products == null)
                return BadRequest("This seller doesn't have products or id is wrong");
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Search query is empty");

            var products = await _productService.GetFilteredByName(name);
            foreach (var p in products)
            {
                Console.WriteLine($"!returned: {p.Id} - {p.Name} - available: {p.IsAvailable}");
            }
            //путь к Image для  продукта
            foreach (var product in products)
            {
                product.ImagePath = _utilsService.GetFirstImageUrl(product.ImagesPath, Request);
            }
            return Ok(products);
        }

        [HttpPost]        
        public async Task<IActionResult> Create([FromForm] ProductCreateModel model)
        {
            if (model.Images == null || model.Images.Count == 0)
                return BadRequest("No images uploaded.");

            var productName = model.Name?.Trim();
            if (string.IsNullOrWhiteSpace(productName))
                return BadRequest("Product name is required.");

            // Папка: wwwroot/images/{ProductName}
            var imagesFolder = Path.Combine("wwwroot", "images", productName);
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            var imagePaths = new List<string>();

            foreach (var image in model.Images)
            {
                var fileName = Path.GetFileName(image.FileName);
                var savePath = Path.Combine(imagesFolder, fileName);
                using var stream = new FileStream(savePath, FileMode.Create);
                await image.CopyToAsync(stream);

                imagePaths.Add($"/images/{productName}/{fileName}");
            }

            var product = new ProductDTO
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                Rating = 0, 
                ImagesPath = $"images/{productName}", // Папка
                ImagePath = imagePaths.FirstOrDefault(), // Первая картинка (если нужно отображать превью)
                StoreId = model.StoreId,
                SubcategoryId = model.SubcategoryId,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId,
                IsAvailable = true
            };

            await _productService.Create(product);
            return Ok("Product created");
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductUpdateModel product)
        {
            if (id != product.Id)
                return BadRequest("Product ID mismatch");
            var oldProduct = await _productService.Get(id);
            var oldImagesFolder = Path.Combine("wwwroot", "images", oldProduct.Name);
            var newImagesFolder = Path.Combine("wwwroot", "images", product.Name);
            if (Directory.Exists(oldImagesFolder))
                Directory.Delete(oldImagesFolder, true);
            Directory.CreateDirectory(newImagesFolder);
            foreach (var image in product.Images)
            {
                var fileName = Path.GetFileName(image.FileName);
                var savePath = Path.Combine(newImagesFolder, fileName);
                using var stream = new FileStream(savePath, FileMode.Create);
                await image.CopyToAsync(stream);
            }
            var productDTO = ConvertToDTO(product);
            productDTO.ImagesPath = $"images/{product.Name}";
            productDTO.Rating = oldProduct.Rating;
            productDTO.IsAvailable = oldProduct.IsAvailable;
            productDTO.StoreId = oldProduct.StoreId;
            await _productService.Update(productDTO);
            return Ok("Product updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.Delete(id);
            return Ok("Product deleted");
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Get(int id)
        {
             if(id <= 0)
                return BadRequest("Invalid product ID");
            var product = await _productService.Get(id);
            if (product == null)
                return NotFound("Product not found");
            product.ImagePath = _utilsService.GetFirstImageUrl(product.ImagesPath, Request);
            product.ImagesUrls = _utilsService.GetImagesUrl(product.ImagesPath, Request);
            return Ok(product);
        }

        [HttpGet("similar/{productId}")]
        public async Task<IActionResult> GetSimilarProducts(int productId)
        {
            var product = await _productService.Get(productId);
            if (product == null)
                return NotFound("Product not found");

            var all = await _productService.GetAll();

            var similar = all
                .Where(p => p.Id != productId &&
                    (p.CategoryId == product.CategoryId ||
                     p.SubcategoryId == product.SubcategoryId))
                .Take(10)
                .ToList();

            foreach (var item in similar)
                item.ImagePath = _utilsService.GetFirstImageUrl(item.ImagesPath, Request);

            return Ok(similar);
        }


        [NonAction]
        public ProductDTO ConvertToDTO(ProductUpdateModel model) {
            ProductDTO productDTO = new ProductDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                StoreId = model.StoreId,
                SubcategoryId = model.SubcategoryId,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId,
                ImagesPath = model.ImagesPath

            };
            return productDTO;
        }

        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] bool status)
        {
            try
            {
                var product = await _productService.Get(id);
                if (product == null)
                    return NotFound("Product not found");
                product.IsAvailable = status;

                await _productService.Update(product);

                return Ok("Product updated");
            }
            catch(Exception ex)
            {
                return BadRequest($"Error updating product: {ex.Message}");
            }

            
        }
        [HttpPost("get-by-ids")]
        public async Task<IActionResult> GetProductsByIds([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest("No product IDs provided");
            var products = new List<ProductDTO>();
            foreach (var id in ids)
            {
                var product = await _productService.Get(id);
                if (product != null)
                {
                    product.ImagePath = _utilsService.GetFirstImageUrl(product.ImagesPath, Request);
                    products.Add(product);
                }
            }
            return Ok(products);
        }

    }
}