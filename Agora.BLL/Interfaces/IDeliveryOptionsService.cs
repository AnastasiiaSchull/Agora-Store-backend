using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IDeliveryOptionsService
    {
        Task<IQueryable<DeliveryOptionsDTO>> GetAll();
        Task<DeliveryOptionsDTO> Get(int id);
        Task Create(DeliveryOptionsDTO deliveryOptionsDTO);
        Task Update(int id, DeliveryOptionsDTO deliveryOptionsDTO);
        Task Delete(int id);
        Task<IEnumerable<DeliveryOptionsDTO>> GetBySellerId(int sellerId);
        Task<IEnumerable<DeliveryOptionsDTO>> GetByStoreId(int storeId);
        Task DeleteAllBySellerId(int sellerId);
    }
}
