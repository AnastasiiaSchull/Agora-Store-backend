using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface ILiqpayService
    {
        LiqpayFormDTO GetLiqPayModelForOrder(string orderId, decimal amount);
        LiqpayFormDTO GetLiqPayModelForGiftCard(string giftCardId, decimal amount);
        bool VerifySignature(string data, string receivedSignature);

        string GetSignature(string data);
    }
}
