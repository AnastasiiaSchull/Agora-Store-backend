using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/delivery-options")]
    public class DeliveryOptionsController : ControllerBase
    {
        private readonly IDeliveryOptionsService _deliveryOptionsService;

        public DeliveryOptionsController (IDeliveryOptionsService deliveryOptionsService)
        {
            _deliveryOptionsService = deliveryOptionsService;
        }

        [HttpGet("{storeId}")]
        public async Task<IActionResult> GetByStoreId(int storeId)
        {
            var options = await _deliveryOptionsService.GetBySellerId(storeId);
            return Ok(options);
        }

        [HttpDelete("all/{sellerId}")]
        public async Task<IActionResult> DeleteAllBySellerId(int sellerId)
        {
            await _deliveryOptionsService.DeleteAllBySellerId(sellerId);
            return NoContent();
        }

        [HttpPost("create-delivery-option")]
        public async Task<IActionResult> Create([FromBody] DeliveryOptionsDTO deliveryOptionsDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _deliveryOptionsService.Create(deliveryOptionsDTO);
                return Ok();
            }
            catch (ValidationExceptionFromService ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the delivery option.");
            }
        }
    }
}
