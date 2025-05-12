using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/seller-reviews")]
    [ApiController]
    public class SellerReviewController : ControllerBase
    {
        private readonly ISellerReviewService _sellerReviewService;

        public SellerReviewController(ISellerReviewService sellerReviewService)
        {
            _sellerReviewService = sellerReviewService;
        }

        [HttpGet("{sellerId}/reviews")]
        public async Task<IActionResult> GetSellerReviews(int sellerId)
        {
            var reviews = await _sellerReviewService.GetAll();
            var sellerReviews = reviews.Where(r => r.SellerId == sellerId)
                .Select(r => new
                {
                    UserName = r.Customer.UserDTO.Name,
                    UserSunname = r.Customer.UserDTO.Surname,
                    r.Comment,
                    r.Rating, 
                    Date = r.Date.ToString()
                })
                .ToList();

            return Ok(sellerReviews);
        }
        [HttpPost("{sellerId}/review")]
        public async Task<IActionResult> CreateSellerReview(int sellerId, [FromBody] SellerReviewDTO reviewDto)
        {
            if (sellerId != reviewDto.SellerId)
                return BadRequest("Seller ID mismatch.");           

            var review = new SellerReviewDTO
            {
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating,
                Date = DateOnly.FromDateTime(DateTime.Today),                
                SellerId = reviewDto.SellerId,
                CustomerId = reviewDto.CustomerId 
            };

            await _sellerReviewService.Create(review);

            return Ok("Review submitted successfully.");
        }

    }
}
