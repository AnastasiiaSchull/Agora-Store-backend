using System.Text;
using System.Text.RegularExpressions;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Agora.DAL.Entities;
using Agora.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Sprache;

namespace Agora.Controllers
{
    [Route("api/gift-card")]
    [ApiController]
    public class GiftCardController : ControllerBase
    {
        private readonly IGiftCardService _giftCardService;
        private readonly ILiqpayService _liqpayService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentMethodService _paymentMethodService;

        public GiftCardController(IGiftCardService giftCardService, ILiqpayService liqpayService,
            IPaymentService paymentService, IPaymentMethodService paymentMethodService)
        {
            _giftCardService = giftCardService;
            _liqpayService = liqpayService;
            _paymentService = paymentService;
            _paymentMethodService = paymentMethodService;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuyGiftCard(GiftCardForm model)
        {
            try
            {
                GiftCardDTO giftCardDTO = new GiftCardDTO
                {
                    Balance = model.Balance,
                    ExpirationDate = model.ExpirationDate,
                };
                var code = GenerateFormattedCode();
                giftCardDTO.Code = code;
                var giftCardId = await _giftCardService.Create(giftCardDTO);
                await CreatePayment(model.Balance, model.CustomerId, giftCardId);
                var formModel =  _liqpayService.GetLiqPayModelForGiftCard(giftCardId.ToString(), model.Balance);
                //await CreatePayment(model, orderId);
                return Ok(formModel);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
           
        }

        [HttpPost("redirect")]
        public async Task<IActionResult> HandleLiqpayCallback()
        {
            string rawBody;
            string data = "";
            string signature = "";
            Dictionary<string, string> jsonResponse = null;
            string locale = Request.Cookies["NEXT_LOCALE"] ?? "en";
            var redirectUrl = $"http://localhost:3000/{locale}/account/gift-card/";
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
                
                if (parsedQuery.TryGetValue("data", out StringValues dataValues) && parsedQuery.TryGetValue("signature", out StringValues signatureValues))
                {
                    if (!string.IsNullOrEmpty(dataValues.ToString()) && !string.IsNullOrEmpty(signatureValues.ToString()))
                    {
                        data = dataValues.ToString();
                        signature = signatureValues.ToString();
                    }
                }
                else
                {
                    //delete gift card, payment and payment method rows in DB
                    return Redirect(errorUrl);
                }

                var json = Encoding.UTF8.GetString(Convert.FromBase64String(data));
                jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                var mySignature = _liqpayService.GetSignature(data);

                if (mySignature != signature)
                {
                    //delete gift card, payment and payment method rows in DB
                    return Redirect(errorUrl);
                }
                if (jsonResponse != null && jsonResponse.ContainsKey("status") &&
                (jsonResponse["status"] == "success" || jsonResponse["status"] == "sandbox"))
                {
                    var id = jsonResponse["order_id"];
                    id = Regex.Replace(id, "[^0-9]", "");
                    int giftCardId = Int32.Parse(id);
                    var payment = await _paymentService.GetByGiftCardId(giftCardId);
                    payment.Status = Enums.PaymentStatus.Completed;
                    payment.Data = data;
                    payment.Signature = signature;
                    await _paymentService.Update(payment);

                    return Redirect(redirectUrl + giftCardId);
                }
            }
            catch (Exception ex)
            {
                //delete gift card, payment and payment method rows in DB
                return BadRequest("Invalid data format or decoding error.");
            }
            return Redirect(errorUrl);

        }

        [NonAction]
        public string GenerateFormattedCode()
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            var random = new Random();
            var result = new StringBuilder();

            for (int g = 0; g < 3; g++)
            {
                if (g > 0)
                    result.Append("-");

                for (int i = 0; i < 4; i++)
                {
                    result.Append(chars[random.Next(chars.Length)]);
                }
            }

            return result.ToString();
        }

        [NonAction]
        public async Task CreatePayment(decimal amount, int customerId, int giftCardId)
        {
            PaymentMethodDTO paymentMethodDTO = new PaymentMethodDTO
            {
                GiftCardId = giftCardId
            };
            var paymentMethodId = await _paymentMethodService.Create(paymentMethodDTO);

            PaymentDTO paymentDTO = new PaymentDTO
            {
                Amount = amount,
                Status = Enums.PaymentStatus.Pending,
                TransactionDate = DateTime.Now,
                CashbackUsed = 0,
                CustomerId = customerId,
                PaymentMethodId = paymentMethodId

            };
            await _paymentService.Create(paymentDTO);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest("Wrong id");
                var giftCard = await _giftCardService.Get(id);
                if (giftCard != null)
                    return Ok(giftCard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error: " + ex.Message });
            }
            return BadRequest();
        }
    }
}
