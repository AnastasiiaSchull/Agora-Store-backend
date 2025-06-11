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

        [HttpGet("option/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var option = await _deliveryOptionsService.Get(id);
                return Ok(option);
            }
            catch (ValidationExceptionFromService ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("all/{sellerId}")]
        public async Task<IActionResult> DeleteAllBySellerId(int sellerId)
        {
            await _deliveryOptionsService.DeleteAllBySellerId(sellerId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _deliveryOptionsService.Delete(id);
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

        [HttpPut("update-delivery-option/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DeliveryOptionsDTO deliveryOptionsDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deliveryOptionsDTO.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            try
            {
                await _deliveryOptionsService.Update(id, deliveryOptionsDTO);
                return Ok(new { message = "Delivery option updated successfully." });
            }
            catch (ValidationExceptionFromService ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
