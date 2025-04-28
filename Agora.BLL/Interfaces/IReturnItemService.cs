using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IReturnItemService
    {
        Task<IQueryable<ReturnItemDTO>> GetAll();
        Task<ReturnItemDTO> Get(int id);
        Task Create(ReturnItemDTO returnItemDTO);
        Task Update(ReturnItemDTO returnItemDTO);
        Task Delete(int id);
    }
}
