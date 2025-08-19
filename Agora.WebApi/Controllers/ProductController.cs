using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Agora.Models;
using Microsoft.AspNetCore.Mvc;
using Tensorflow;

namespace Agora.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IDeliveryOptionsService _deliveryOptionsService;
        private readonly ITranslationService _translationService;
        private readonly IUtilsService _utilsService;
        private readonly ISellerService _sellerService;

        public ProductController(IProductService productService, IUtilsService utilsService, IDeliveryOptionsService deliveryOptionsService, ITranslationService translationService, ISellerService sellerService)
        {
            _productService = productService;
            _utilsService = utilsService;
            _deliveryOptionsService = deliveryOptionsService;
            _translationService = translationService;
            _sellerService = sellerService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAll();
            foreach (var item in products)
                item.ImagePath = _utilsService.GetFirstImageUrl(item.ImagesPath, Request);
            return Ok(products);
        }

        [HttpGet("get-all-by-store/{storeId}")]
        public async Task<IActionResult> GetAllProductsBySeller(int storeId )
        {
            var products = await _productService.GetAllProductsByStore(storeId);
            foreach(var item in products)
                item.ImagePath = _utilsService.GetFirstImageUrl(item.ImagesPath, Request);
            if (products == null)
                return BadRequest("This store doesn't have products or id is wrong");
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Search query is empty");

            var products = await _productService.GetFilteredByName(name);
            //foreach (var p in products)
            //{
            //    Console.WriteLine($"!returned: {p.Id} - {p.Name} - available: {p.IsAvailable}");
            //}
            //путь к Image для  продукта
            foreach (var product in products)
            {
                product.ImagePath = _utilsService.GetFirstImageUrl(product.ImagesPath, Request);
            }
            return Ok(products);
        }

        [HttpGet("search-multilang")] 
        public async Task<IActionResult> SearchMultilingual([FromQuery] string query, [FromQuery] string locale)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query is empty");

            // если не английский -переводим запрос
            string translatedQuery = locale.ToLower() == "en"
                ? query
                : await _translationService.Translate(query, locale);

            var products = await _productService.GetFilteredByName(translatedQuery);

            foreach (var product in products)
                product.ImagePath = _utilsService.GetFirstImageUrl(product.ImagesPath, Request);

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

        [HttpPost("recommendations")]
        public async Task<IActionResult> GetRecommendations([FromBody] RecommendationRequestModel request)
        {
            var allRecommendations = new List<ProductDTO>();

            // обрабатываем поисковые запросы
            foreach (var query in request.Queries.Distinct())
            {
                var results = await _productService.GetFilteredByName(query);
                var selected = results
                    .Where(p => p.IsAvailable)
                    .OrderByDescending(p => p.Rating)
                    .Take(8)
                    .ToList();

                allRecommendations.AddRange(selected);
            }

            // обрабатываем товары из корзины
            foreach (var productId in request.BasketIds.Distinct())
            {
                var similar = await _productService.GetSimilarProducts(productId);
                allRecommendations.AddRange(similar.Take(8));
            }

            // убираем дубликаты поId
            var unique = allRecommendations
                .GroupBy(p => p.Id)
                .Select(g => g.First())
                .ToList();
           
            foreach (var item in unique)
                item.ImagePath = _utilsService.GetFirstImageUrl(item.ImagesPath, Request);

            return Ok(unique);
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

                if (product.Store == null || product.Store.SellerId == null)
                    return BadRequest("Product store or seller information is missing");

                var seller = await _sellerService.Get(product.Store.SellerId.Value);
                if (seller.IsBlocked)
                    return BadRequest("Blocked sellers cannot change product availability.");

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
            var products = new List<ProductForBasket>();
            foreach (var id in ids)
            {
                var product = await _productService.Get(id);
                if (product != null)
                {
                    product.ImagePath = _utilsService.GetFirstImageUrl(product.ImagesPath, Request);
                    var convertedProduct = ConvertToProductForBasket(product);

                    var deliveryOptions = await _deliveryOptionsService.GetBySellerId(product.Store.SellerId.Value);
                    convertedProduct.DeliveryOptions = deliveryOptions.ToList();
                    products.Add(convertedProduct);
                }
            }
            return Ok(products);
        }

        [HttpPost("get-by-ids-for-checkout")]
        public async Task<IActionResult> GetProductsForCheckout([FromBody] List<Cart> cart)
        {

            if (cart == null || cart.Count == 0)
                return BadRequest("No data provided");
            var products = new List<ProductForCheckout>();
            foreach (var item in cart)
            {
                var product = await _productService.Get(item.ProductId);
                if (product != null)
                {
                    product.ImagePath = _utilsService.GetFirstImageUrl(product.ImagesPath, Request);
                    var convertedProduct = ConvertToProductForCheckout(product);

                    var deliveryOption = await _deliveryOptionsService.Get(item.DeliveryOptionId);

                    convertedProduct.DeliveryOption = deliveryOption;
                    convertedProduct.Quantity = item.Quantity;
                    products.Add(convertedProduct);
                }
            }
            return Ok(products);
        }

        [NonAction]
        public ProductForBasket ConvertToProductForBasket(ProductDTO model)
        {
            ProductForBasket product = new ProductForBasket
            {
                Id = model.Id,
                Name = model.Name, 
                Price = model.Price,
                DiscountedPrice = model.DiscountedPrice,
                StockQuantity = model.StockQuantity,
                StoreId = model.StoreId,
                Rating = model.Rating,
                IsAvailable = model.IsAvailable,
                ImagePath = model.ImagePath,
                


            };
            return product;
        }

        [NonAction]
        public ProductForCheckout ConvertToProductForCheckout(ProductDTO model)
        {
            ProductForCheckout product = new ProductForCheckout
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                Quantity = model.StockQuantity,
                StoreName = model.Store.Name,
                IsAvailable = model.IsAvailable,
                ImagePath = model.ImagePath,

            };
            return product;
        }

        [HttpGet("image/{*imagePath}")]
        public IActionResult GetImage(string imagePath)
        {
            try
            {
                var decodedPath = Uri.UnescapeDataString(imagePath);

                var imagesFolder = Path.Combine("wwwroot", "images");
                var fullPath = Path.Combine(imagesFolder, decodedPath);

                if (!fullPath.StartsWith(imagesFolder))
                {
                    return BadRequest("Invalid image path");
                }

                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound("Image not found");
                }

                var fileInfo = new FileInfo(fullPath);
                var contentType = GetContentType(fileInfo.Extension);

                var fileBytes = System.IO.File.ReadAllBytes(fullPath);

                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error retrieving image");
            }
        }

        [NonAction]
        private string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }

        [HttpGet("get-filtered-products-by-store/id={storeId}&filterField={field}&filterValue={value}")]
        public async Task<IActionResult> GetFiltredOrders(int storeId, string field, string value)
        {

            IEnumerable<ProductDTO> products = await _productService.GetFiltredProducts(storeId, field, value);
            if (products == null)
                return new JsonResult(new { message = "Server error!" }) { StatusCode = 500 };
            foreach (var item in products)
                item.ImagePath = _utilsService.GetFirstImageUrl(item.ImagesPath, Request);
            return Ok(products);
        }
    }
}