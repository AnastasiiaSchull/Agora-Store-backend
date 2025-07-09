using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/sellers")]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSellers()
        {
            var sellers = await _sellerService.GetAll();
            return Ok(sellers.ToList());
        }

        [HttpPut("block/{id}")]
        public async Task<IActionResult> BlockSeller(int id)
        {
            try
            {
                await _sellerService.BlockSeller(id);
                return Ok(new { message = "Seller blocked and products disabled." });
            }
            catch (ValidationExceptionFromService ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("unblock/{id}")]
        public async Task<IActionResult> UnblockSeller(int id)
        {
            try
            {
                await _sellerService.UnblockSeller(id);
                return Ok(new { message = "Seller unblocked." });
            }
            catch (ValidationExceptionFromService ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
