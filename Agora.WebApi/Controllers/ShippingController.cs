using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.Models;
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
        private readonly IStoreService _storeService;


        public ShippingController(IOrderItemService orderItemService, IUtilsService utilsService, IShippingService shippingService, IStoreService storeService)

        {
            _orderItemService = orderItemService;
            _utilsService = utilsService;
            _shippingService = shippingService;
            _storeService = storeService;
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

        [HttpPost]
        public async Task<IActionResult> Receive([FromBody] DhlPayload payload)
        {
            try
            {
                var status = payload.Status;
                ShippingDTO shipping = await _shippingService.GetByTrackingNumber(payload.TrackingNumber);
                if(shipping != null)
                {
                    shipping.Status = status.ToString();
                    await _shippingService.Update(shipping);

                    if(status == Enums.ShippingStatus.InTransit || status == Enums.ShippingStatus.Delivered)
                    {
                        var orderItem = await _orderItemService.Get(shipping.OrderItemId.Value);
                     
                        if (orderItem != null)
                        {
                            if(status == Enums.ShippingStatus.InTransit)
                            {
                                orderItem.Status = Enums.OrderStatus.Shipped.ToString();
                                await _orderItemService.Update(orderItem);
                            }
                            else if (status == Enums.ShippingStatus.Delivered)
                            {
                                orderItem.Status = Enums.OrderStatus.Delivered.ToString();
                                await _orderItemService.Update(orderItem);
                                var store = await _storeService.Get(orderItem.ProductDTO.Store.Id);
                                store.FundsBalance += orderItem.ProductDTO.Price * orderItem.Quantity;
                                await _storeService.Update(store);
                            }


                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
            return Ok();

        }


    }
}
