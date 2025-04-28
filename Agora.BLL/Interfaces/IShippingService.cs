using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IShippingService
    {
        Task<IQueryable<ShippingDTO>> GetAll();
        Task<ShippingDTO> Get(int id);
        Task Create(ShippingDTO sippingDTO);
        Task Update(ShippingDTO sippingDTO);
        Task Delete(int id);
    }
}
