using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.Enums;
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
        private readonly IReturnService _returnService;
        private readonly IReturnItemService _returnItemService;

        public ReturnProductController(IEmailService emailService, IUserService userService, ISellerService sellerService,
            IOrderItemService orderItemService, IReturnService returnService, IReturnItemService returnItemService)
        {
            _emailService = emailService;
            _userService = userService;
            _sellerService = sellerService;
            _orderItemService = orderItemService;
            _returnService = returnService;
            _returnItemService = returnItemService;
        }

        [HttpPost]
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

                var returnDTO = new ReturnDTO
                {
                    ReturnDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Status = ReturnStatus.Requested,
                    RefundAmount = orderItem.PriceAtMoment,
                    OrderId = orderItem.OrderDTO.Id,
                    CustomerId = orderItem.OrderDTO.CustomerDTO.Id
                };

                int createdReturnId = await _returnService.Create(returnDTO);

                var returnItemDTO = new ReturnItemDTO
                {
                    Quantity = orderItem.Quantity,
                    Reason = model.Message,
                    ReturnId = createdReturnId,
                    ProductId = orderItem.ProductDTO.Id
                };

                await _returnItemService.Create(returnItemDTO);

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
