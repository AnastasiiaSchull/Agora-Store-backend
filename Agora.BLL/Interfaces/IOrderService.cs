using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<IQueryable<OrderDTO>> GetAll();
       /* Task<IEnumerable<OrderDTO>> GetFilteredBy(string filter);*/ // to be continued
        Task<OrderDTO> Get(int id);
        Task Create(OrderDTO orderDTO);
        Task Update(OrderDTO orderDTO);
        Task Delete(int id);
    }
}
