using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IQueryable<Payment>> GetAll();
        Task<Payment> Get(int id);
        Task<Payment> GetByOrderId(int orderId);
        Task<Payment> GetByGiftCardId(int giftCardId);
        Task Create(Payment item);
        void Update(Payment item);
        Task Delete(int id);
    }
}
