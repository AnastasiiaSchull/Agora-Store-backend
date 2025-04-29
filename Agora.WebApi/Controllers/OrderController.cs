using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;


        public OrderController(IOrderItemService orderItemService)

        {
            _orderItemService = orderItemService;
        }

        [HttpGet("get-new-orders/{storeId}")]
        public async Task<IActionResult> GetNewOrders(int storeId)
        {
            IEnumerable<OrderItemDTO> newOrders = await _orderItemService.GetNewOrders(storeId);
            if(newOrders == null)
                return new JsonResult(new { message = "Server error!" }) { StatusCode = 500 };
            return Ok(newOrders);
        }


        [HttpGet("get-orders-by-store/{storeId}")]
        public async Task<IActionResult> GetOrdersByStore(int storeId)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("inside controller");
            IEnumerable<OrderItemDTO> orders = await _orderItemService.GetAllByStore(storeId);
            if (orders == null)
                return new JsonResult(new { message = "Server error!" }) { StatusCode = 500 };
            return Ok(orders);
        }



    }
}
