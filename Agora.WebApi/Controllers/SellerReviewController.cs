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
                    UserName = r.Customer.User.Name,
                    UserSunname = r.Customer.User.Surname,
                    r.Comment,
                    r.Rating, 
                    Date = r.Date.ToString()
                })
                .ToList();

            return Ok(sellerReviews);
        }        
    }
}
