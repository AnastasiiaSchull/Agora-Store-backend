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
        private readonly IUtilsService _utilsService;

        public ReturnProductController(IEmailService emailService, IUserService userService, ISellerService sellerService,
            IOrderItemService orderItemService, IReturnService returnService, IReturnItemService returnItemService, IUtilsService utilsService)
        {
            _emailService = emailService;
            _userService = userService;
            _sellerService = sellerService;
            _orderItemService = orderItemService;
            _returnService = returnService;
            _returnItemService = returnItemService;
            _utilsService = utilsService;
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
                    return BadRequest("Seller not found");

                var returnDTO = new ReturnDTO
                {
                    ReturnDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Status = ReturnStatus.Requested.ToString(),
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
                    ProductId = orderItem.ProductDTO.Id,
                    OrderItemId = orderItem.Id
                };

                int createdReturnItemId = await _returnItemService.Create(returnItemDTO);                

                await _orderItemService.UpdateStatus(orderItem.Id, OrderStatus.RefundRequested.ToString());

                var subject = $"Return request for item #: {model.IdItem} from customer {orderItem.OrderDTO.CustomerDTO.UserDTO.Name} {orderItem.OrderDTO.CustomerDTO.UserDTO.Surname}";
                var body = $@"
        <p><strong>From:</strong> {orderItem.OrderDTO.CustomerDTO.UserDTO.Email}</p>
        <p><strong>Item ID:</strong> {model.IdItem}</p>
        <p><strong>Return Item ID:</strong> {createdReturnItemId}</p>
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

        [HttpGet("get-new-returns-by-store/{storeId}")]
        public async Task<IActionResult> GetNewReturns(int storeId)
        {
            IEnumerable<ReturnItemDTO> newReturns = await _returnItemService.GetNewReturns(storeId);
            
            if (newReturns == null)
                return Ok(new List<object>());
            return Ok(newReturns);
        }

        [HttpGet("get-returns-by-store/{storeId}")]
        public async Task<IActionResult> GetReturnsByStore(int storeId)
        {
            IEnumerable<ReturnItemDTO> returns = await _returnItemService.GetAllByStore(storeId);

            if (returns == null)
                return Ok(new List<object>());
            return Ok(returns);
        }

        [HttpGet("get-filtered-returns-by-store/id={storeId}&filterField={field}&filterValue={value}")]
        public async Task<IActionResult> GetFiltredreturns(int storeId, string field, string value)
        {

            IEnumerable<ReturnItemDTO> returns = await _returnItemService.GetFiltredReturns(storeId, field, value);
            if (returns == null)
                return Ok(new List<object>());
            return Ok(returns);
        }

        [HttpGet("get-return-details/{returnId}")]
        public async Task<IActionResult> GetReturnDetails(int returnId)
        {
            var dto = await _returnItemService.GetReturnItemDetails(returnId);
            if (dto == null)
                return NotFound(new { message = "Return item not found" });
            dto.ProductImage = _utilsService.GetFirstImageUrl(dto.ProductImage, Request);

            return Ok(dto);
        }

    }
}
