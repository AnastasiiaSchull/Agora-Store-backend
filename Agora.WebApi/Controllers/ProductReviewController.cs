using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Agora.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/product-reviews")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;
        public ProductReviewController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }

    [HttpGet("store/{storeId}")]
    public async Task<ActionResult> GetReviewsByStoreId(int storeId)
    {
        var reviews = await _productReviewService.GetReviewsByStoreId(storeId);

        if (reviews == null || !reviews.Any())
            return NotFound("No reviews found for this store.");

        var productReviews = reviews
        .Select(r => new
        {
            UserName = r.Customer?.UserDTO?.Name,
            UserSurname = r.Customer?.UserDTO?.Surname,
            ProductName = r.Product?.Name,
            ProductDescription = r.Product?.Description,
            ProductImage = GetFirstProductImageUrl(r.Product?.ImagesPath, Request),
            ProductRating = r.Product?.Rating,
            r.Comment,
            r.Rating,
            Date = r.Date.ToString()
        })
        .ToList();
        return Ok(productReviews);
        }

        private string? GetFirstProductImageUrl(string? folderPath, HttpRequest request)
        {
            if (string.IsNullOrEmpty(folderPath))
                return null;

            var fullFolderPath = Path.Combine("wwwroot", folderPath);

            if (!Directory.Exists(fullFolderPath))
                return null;

            var firstImage = Directory
                .GetFiles(fullFolderPath)
                .FirstOrDefault(file =>
                    file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".webp", StringComparison.OrdinalIgnoreCase));

            if (firstImage == null)
                return null;

            var fileName = Path.GetFileName(firstImage);
            return $"{request.Scheme}://{request.Host}/{folderPath}/{fileName}";
        }
    }
}
