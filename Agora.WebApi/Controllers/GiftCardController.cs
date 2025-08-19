using System.Text;
using System.Text.RegularExpressions;
using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
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
        private readonly IConfiguration _config;
        private readonly ISecureService _secureService;

        public GiftCardController(IGiftCardService giftCardService, ILiqpayService liqpayService,
            IPaymentService paymentService, IPaymentMethodService paymentMethodService, IConfiguration config, ISecureService secureService)
        {
            _giftCardService = giftCardService;
            _liqpayService = liqpayService;
            _paymentService = paymentService;
            _paymentMethodService = paymentMethodService;
            _config = config;
            _secureService = secureService;
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
                    IsAvailable = true
                };
                var code = GenerateFormattedCode();
                giftCardDTO.Code = code;
                var giftCardId = await _giftCardService.Create(giftCardDTO);
                await CreatePayment(model.Balance, model.CustomerId, giftCardId);
                var formModel = _liqpayService.GetLiqPayModelForGiftCard(giftCardId.ToString(), model.Balance);
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
            var baseUrl = _config["FRONTEND_URL"]!;
            var redirectUrl = $"{baseUrl}/en/account/gift-card/";
            var errorUrl = $"{baseUrl}/en/error/payment-went-wrong";
            

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

            var mySignature = _liqpayService.GetSignature(data);
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(data));
            jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var id = jsonResponse["order_id"];
            id = Regex.Replace(id, "[^0-9]", "");
            int giftCardId = Int32.Parse(id);

            try
            {
                if (mySignature != signature)
                {

                    //delete gift card, payment and payment method rows in DB
                    await DeleteGiftCard(giftCardId);
                    return Redirect(errorUrl);
                }
                if (jsonResponse != null && jsonResponse.ContainsKey("status") &&
                (jsonResponse["status"] == "success" || jsonResponse["status"] == "sandbox"))
                {

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
                await DeleteGiftCard(giftCardId);
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
            await DeleteGiftCard(giftCardId);
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
            PaymentDTO paymentDTO = new PaymentDTO
            {
                Amount = amount,
                Status = Enums.PaymentStatus.Pending,
                TransactionDate = DateTime.Now,
                CashbackUsed = 0,
                CustomerId = customerId,
                GiftCardId = giftCardId

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

        [NonAction]
        public async Task DeleteGiftCard(int giftCardId)
        {
            try
            {
                var payment = await _paymentService.GetByGiftCardId(giftCardId);
                if (payment == null)
                    throw new Exception("Wrong gift card id");
                await _paymentService.Delete(payment.Id);
                await _paymentMethodService.Delete(payment.PaymentMethodId.Value); //check it here
                await _giftCardService.Delete(giftCardId);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }

        [HttpGet("get-by-code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return BadRequest("Wrong code");
                var giftCard = await _giftCardService.GetByCode(code);
                if (giftCard == null)
                    return BadRequest("There is no item with this code");
                if (giftCard.ExpirationDate < DateOnly.FromDateTime(DateTime.Now))
                    return BadRequest("This gift card is expired");
                if (giftCard.IsAvailable == false)
                    return BadRequest("This card isn't available");
                return Ok(giftCard);
            }
            catch(ValidationExceptionFromService ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error: " + ex.Message });
            }
            
        }
    }
}
