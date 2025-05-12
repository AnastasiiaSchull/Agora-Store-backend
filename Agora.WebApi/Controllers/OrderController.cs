using System.Globalization;
using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Agora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IShippingService _shippingService;
        private readonly IUtilsService _utilsService;


        public OrderController(IOrderItemService orderItemService, IUtilsService utilsService, IShippingService shippingService)

        {
            _orderItemService = orderItemService;
            _utilsService = utilsService;
            _shippingService = shippingService;
        }

        [HttpGet("get-new-orders/{storeId}")]
        public async Task<IActionResult> GetNewOrders(int storeId)
        {
            IEnumerable<OrderItemDTO> newOrders = await _orderItemService.GetNewOrders(storeId);
            if (newOrders == null)
                return new JsonResult(new { message = "Server error!" }) { StatusCode = 500 };
            return Ok(newOrders);
        }


        [HttpGet("get-orders-by-store/{storeId}")]
        public async Task<IActionResult> GetOrdersByStore(int storeId)
        {

            IEnumerable<OrderItemDTO> orders = await _orderItemService.GetAllByStore(storeId);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("WTF");
            if (orders == null)
                return new JsonResult(new { message = "Server error!" }) { StatusCode = 500 };
            return Ok(orders);
        }

        [HttpGet("get-filtered-orders-by-store/id={storeId}&filterField={field}&filterValue={value}")]
        public async Task<IActionResult> GetFiltredOrders(int storeId, string field, string value)
        {

            IEnumerable<OrderItemDTO> orders = await _orderItemService.GetFiltredOrders(storeId, field, value);
            if (orders == null)
                return new JsonResult(new { message = "Server error!" }) { StatusCode = 500 };
            return Ok(orders);
        }

        [HttpGet("get-order-details/{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            try
            {
                if (orderId == null)
                    return new JsonResult(new { message = "Invalid order ID!" }) { StatusCode = 400 };
                OrderItemDTO order = await _orderItemService.Get(orderId);
                if (order == null)
                    return new JsonResult(new { message = "Invalid order!" }) { StatusCode = 400 };

                order.ProductDTO.ImagePath = _utilsService.GetFirstImageUrl(order.ProductDTO.ImagesPath, Request);

                //order.ShippingDTO.ShipDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(De)); //migth be needed
                return Ok(order);
            }
            catch (ValidationExceptionFromService ex)
            {
                return new JsonResult(new { message = ex.Message }) { StatusCode = 400 };

            }
           
            
        }




    }
}
