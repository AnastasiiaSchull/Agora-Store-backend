using System.Text;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Agora.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Sprache;

namespace Agora.Controllers
{
    [Route("api/checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ILiqpayService _liqpayService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;
        private readonly IShippingService _shippingService;

        public CheckoutController(ILiqpayService liqpayService, IOrderService orderService, IOrderItemService orderItemService, 
            IProductService productService, IPaymentService paymentService, IShippingService shippingService)
        {
            _liqpayService = liqpayService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _productService = productService;
            _paymentService = paymentService;
            _shippingService = shippingService;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PaymentModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest("No data");

                var selectedAddress = HttpContext.Request.Headers["selectedAddress"];
                int selectedAddressId = Int32.Parse(selectedAddress);
                var orderId = await Purchase(model, selectedAddressId);
                if (orderId != null)
                {
                    var formModel = _liqpayService.GetLiqPayModel(orderId.ToString(), model.Amount);
                    await CreatePayment(model, orderId);
                    return Ok(formModel);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error processing payment: {ex.Message}");
            }
        }

        [HttpPost("redirect")]
        public async Task<IActionResult> HandleLiqpayCallback()
        {
            string rawBody;
            string data = null;
            string signature = null;
            Dictionary<string, string> jsonResponse = null;
            string locale = Request.Cookies["NEXT_LOCALE"] ?? "en";
            var redirectUrl = $"http://localhost:3000/{locale}/account/orders";
            var errorUrl = $"http://localhost:3000/{locale}/500";
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    rawBody = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrEmpty(rawBody))
                {
                    return Redirect(errorUrl);
                }

                Dictionary<string, StringValues> parsedQuery = QueryHelpers.ParseQuery(rawBody);

                if (parsedQuery.TryGetValue("data", out StringValues dataValues))
                {
                    data = dataValues.ToString();
                }

                if (parsedQuery.TryGetValue("signature", out StringValues signatureValues))
                {
                    signature = signatureValues.ToString();
                }

                if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(signature))
                {
                    return Redirect(errorUrl);
                }
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(data));
                jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                var mySignature = _liqpayService.GetSignature(data);

                if (mySignature != signature)
                {
                    return Redirect(errorUrl);
                }
                if (jsonResponse != null && jsonResponse.ContainsKey("status") &&
                (jsonResponse["status"] == "success" || jsonResponse["status"] == "sandbox"))
                {
                    var orderId = jsonResponse["order_id"];
                    int  orderidInt = Int32.Parse(orderId);
                    var paymnet = await _paymentService.GetByOrderId(orderidInt);
                    paymnet.Status = Enums.PaymentStatus.Completed;
                    paymnet.Data = data;
                    paymnet.Signature = signature;
                    await _paymentService.Update(paymnet);

                    return Redirect(redirectUrl);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid data format or decoding error.");
            }
            return Redirect(errorUrl);

        }


        //если пойдёт что-то не так, нужно удалить заказ и все его элементы 
        [NonAction]
        public async Task<int> Purchase(PaymentModel model, int selectedAddress)
        {
            try
            {
                OrderDTO order = new OrderDTO
                {
                    TotalPrice = model.Amount,
                    PaymentDeadline = DateOnly.FromDateTime(DateTime.Now),
                    CustomerId = model.CustomerId
                };
                var orderId = await _orderService.Create(order);


                foreach (var cartItem in model.Cart)
                {
                    var product = await _productService.Get(cartItem.ProductId);

                    OrderItemDTO orderItem = new OrderItemDTO
                    {
                        PriceAtMoment = product.Price,
                        Quantity = cartItem.Quantity,
                        ProductId = cartItem.ProductId,
                        OrderId = orderId,
                        Status = Enums.OrderStatus.Pending.ToString(),
                        Date = DateOnly.FromDateTime(DateTime.Now),
                    };
                    var orderItemId = await _orderItemService.Create(orderItem);

                    ShippingDTO shipping = new ShippingDTO
                    {
                        AddressId = selectedAddress,
                        DeliveryOptionsId = cartItem.DeliveryOptionId,
                        OrderItemId = orderItemId,
                        Status = Enums.ShippingStatus.Pending.ToString(),
                        SellerId = product.SellerId
                    };
                    await _shippingService.Create(shipping);
                }


                return orderId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during purchase: " + ex.Message);
            }

        }


        [NonAction]
        public async Task CreatePayment(PaymentModel model, int orderId)
        {
            PaymentDTO paymentDTO = new PaymentDTO
            {
                Amount = model.Amount,
                Status = Enums.PaymentStatus.Pending,
                TransactionDate = DateTime.Now,
                CashbackUsed = 0,
                OrderId = orderId,
                CustomerId = model.CustomerId

            };
            await _paymentService.Create(paymentDTO);
        }
    }
}
