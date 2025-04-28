using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<IQueryable<PaymentMethodDTO>> GetAll();
        Task<PaymentMethodDTO> Get(int id);
        Task Create(PaymentMethodDTO paymentMethodDTO);
        Task Update(PaymentMethodDTO paymentMethodDTO);
        Task Delete(int id);
    }
}
