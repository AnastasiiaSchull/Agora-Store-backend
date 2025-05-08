using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IShippingService _shippingService;
        private readonly IUtilsService _utilsService;


        public ShippingController(IOrderItemService orderItemService, IUtilsService utilsService, IShippingService shippingService)

        {
            _orderItemService = orderItemService;
            _utilsService = utilsService;
            _shippingService = shippingService;
        }

        [HttpPost("create-shipping")]
        public async Task<IActionResult> CreateShipping([FromBody] ShippingDTO shippingDTO)
        {
            try
            {
                if (shippingDTO == null)
                    return new JsonResult(new { message = "No data!" }) { StatusCode = 400 };
                shippingDTO.Status = "InTransit";
                await _shippingService.Update(shippingDTO);
                return Ok();
            }
            catch (ValidationExceptionFromService ex)
            {
                return new JsonResult(new { message = ex.Message}) { StatusCode = 400 };

            }
            

        }

    }
}
