using System;
using System.Security.Cryptography;
using System.Text;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Agora.BLL.Services
{
    public class LiqpayService : ILiqpayService
    {
        private readonly string _privateKey;
        private readonly string _publicKey;


        public LiqpayService(IConfiguration configuration)
        {
            _privateKey = configuration["Liqpay:PrivateKey"];
            _publicKey = configuration["Liqpay:PublicKey"];
        }

        public LiqpayFormDTO GetLiqPayModelForOrder(string orderId, decimal amount)
        {
            // Fill data for submit them to the view
            var signatureSource = new LiqpayDTO
            {
                PublicKey = _publicKey,
                Version = 3,
                Action = "pay",
                Amount = amount,
                Currency = "USD",
                Description = "Order payment",
                OrderId = "OrderId: " + orderId,
                Sandbox = 1,
                ResultUrl = $"http://localhost:5193/api/checkout/redirect"

            };

            var jsonString = JsonConvert.SerializeObject(signatureSource);
            var dataHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));
            var signatureHash = GetSignature(dataHash);

           
            var model = new LiqpayFormDTO
            {
                Data = dataHash,
                Signature = signatureHash
            };
            return model;
        }
        public LiqpayFormDTO GetLiqPayModelForGiftCard(string giftCardId, decimal amount)
        {
            // Fill data for submit them to the view
            var signatureSource = new LiqpayDTO
            {
                PublicKey = _publicKey,
                Version = 3,
                Action = "pay",
                Amount = amount,    
                Currency = "USD",
                Description = "Gift card payment",
                OrderId = "giftCardId: " + giftCardId,
                Sandbox = 1,
                ResultUrl = $"http://localhost:5193/api/gift-card/redirect" //change URL

            };

            var jsonString = JsonConvert.SerializeObject(signatureSource);
            var dataHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));
            var signatureHash = GetSignature(dataHash);


            var model = new LiqpayFormDTO
            {
                Data = dataHash,
                Signature = signatureHash
            };
            return model;
        }

            public LiqpayFormDTO GetLiqPayModelForWithdrawFunds(decimal amount, string cardNumber, int storeId)
            {
               
                var signatureSource = new LiqpayP2PDTO
                {
                    PublicKey = _publicKey,
                    Version = 3,
                    Action = "p2pcredit",
                    Amount = amount,
                    Currency = "EUR",
                    Description = "P2P payment. Store ID " + storeId,
                    Sandbox = 1,
                    ReceiverCard = cardNumber,
                    OrderId = Guid.NewGuid().ToString(),
                    Ip = "127.0.0.1"
                    //ResultUrl = $"http://localhost:5193/api/gift-card/redirect" //change URL

                };

                var jsonString = JsonConvert.SerializeObject(signatureSource);
                var dataHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));
                var signatureHash = GetSignature(dataHash);


                var model = new LiqpayFormDTO
                {
                    Data = dataHash,
                    Signature = signatureHash
                };
                return model;
            }


            public string GetSignature(string data)
            {
                return Convert.ToBase64String(SHA1.Create()
                    .ComputeHash(Encoding.UTF8.GetBytes(_privateKey + data + _privateKey)));
            }

        public bool VerifySignature(string data, string receivedSignature)
        {
           
            var fullString = _privateKey + data + _privateKey;

            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(fullString));
            var expectedSignature = Convert.ToBase64String(hash);

            return expectedSignature == receivedSignature;
        }

    }
}
