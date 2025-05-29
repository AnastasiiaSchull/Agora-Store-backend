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
    }
}
