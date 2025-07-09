using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Agora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/return")]
    [ApiController]
    public class ReturnProductController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly ISellerService _sellerService;        
        private readonly IOrderItemService _orderItemService;

        public ReturnProductController(IEmailService emailService, IUserService userService, ISellerService sellerService, IOrderItemService orderItemService)
        {
            _emailService = emailService;
            _userService = userService;
            _sellerService = sellerService;           
            _orderItemService = orderItemService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReturnForm([FromForm] ReturnProductViewModel model)
        {
            if (model.IdItem == null)
                return BadRequest("Missing orderItem ID");

            try
            {
                var orderItem = await _orderItemService.Get(model.IdItem.Value);
                
                var seller = await _sellerService.Get(orderItem.ProductDTO.SellerId);
                var sellerUser = await _userService.GetById(seller.UserId.Value);
                if (sellerUser == null)
                    return BadRequest("Seller user not found");

                var subject = $"Return request for item #: {model.IdItem} from customer {orderItem.OrderDTO.CustomerDTO.UserDTO.Name} {orderItem.OrderDTO.CustomerDTO.UserDTO.Surname}";
                var body = $@"
            <p><strong>From:</strong> {orderItem.OrderDTO.CustomerDTO.UserDTO.Email}</p>
            <p><strong>Item ID:</strong> {model.IdItem}</p>
            <p><strong>Message:</strong><br>{model.Message}</p>";

                await _emailService.SendEmailWithAttachmentAsync(sellerUser.Email, subject, body, model.Photo);

                return Ok(new { message = "Return request sent successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Failed to send return request.");
            }
        }
    }
}
