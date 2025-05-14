using System.IO;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Agora_BLL.MLSearchByImage;

namespace Agora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IMLService _mlService;
        private readonly IProductService _productService;

        public PredictionController(IMLService mlService, IProductService productService)
        {
            _mlService = mlService;
            _productService = productService;
        }

        [HttpGet("/{imagePath}")]
        public async Task<IActionResult> PredictAllLabels(string imagePath)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Apple Iphone 16 128", "1.jpg");

            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

            var result = _mlService.PredictAllLabels(imageBytes);

            List<ProductDTO> products = new List<ProductDTO>();
            foreach(var item in result)
            { 
                if(item.Value >= 40.0)
                {
                    var product = await _productService.GetByName(item.Key);
                    products.Add(product);
                }
            }
            return Ok(products);
        }
    }
}
