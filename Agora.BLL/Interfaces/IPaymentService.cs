using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IPaymentService
    {
        Task<IQueryable<PaymentDTO>> GetAll();
        Task<PaymentDTO> Get(int id);
        Task<PaymentDTO> GetByOrderId(int orderId);
        Task<PaymentDTO> GetByGiftCardId(int giftCardId);
        Task Create(PaymentDTO paymentDTO);
        Task Update(PaymentDTO paymentDTO);
        Task Delete(int id);
    }
}
