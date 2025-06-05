using System.IO;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Agora.BLL.ML.MLSearchByImage;

namespace Agora.Controllers
{
    [Route("api/prediction")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IMLService _mlService;
        private readonly IProductService _productService;
        private readonly IUtilsService _utilsService;

        public PredictionController(IMLService mlService, IProductService productService, IUtilsService utilsService)
        {
            _mlService = mlService;
            _productService = productService;
            _utilsService = utilsService;
        }

        [HttpPost("s")]
        public async Task<IActionResult> PredictAllLabels([FromForm] IFormFile image)
        {
            if(image == null)
            {
                return NotFound("Image not found.");
            }


            byte[] imageBytes = await GetBytes(image);
            var result = _mlService.PredictAllLabels(imageBytes); //Получение результатов предсказания в виде строки и значения предстказания в процентном соотношении

            List<ProductDTO> products = new List<ProductDTO>();
            foreach( var item in result)
            { 
                if(item.Value >= 40.0) //Сортирока по процентному соотношению
                {
                    var product = await _productService.GetByName(item.Key); // Получение продукта по имени, которое было предсказано моделью
                    if (product.IsAvailable == true) 
                    {
                        product.ImagePath = _utilsService.GetFirstImageUrl(product.ImagesPath, Request);
                        products.Add(product);
                    }
                    
                }
            }
            return Ok(products);
        }

        [NonAction]
        public async Task<byte[]> GetBytes(IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
