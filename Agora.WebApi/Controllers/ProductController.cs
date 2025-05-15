using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
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

            //путь к Image для  продукта
            foreach (var product in products)
            {
                product.ImagePath = _utilsService.GetFirstImageUrl(product.ImagesPath, Request);
            }
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDTO product)
        {
            await _productService.Create(product);
            return Ok("Product created");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDTO product)
        {
            if (id != product.Id)
                return BadRequest("Product ID mismatch");

            await _productService.Update(product);
            return Ok("Product updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.Delete(id);
            return Ok("Product deleted");
        }
    }
}