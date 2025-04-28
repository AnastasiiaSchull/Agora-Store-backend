using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IReturnService
    {
        Task<IQueryable<ReturnDTO>> GetAll();
        Task<ReturnDTO> Get(int id);
        Task Create(ReturnDTO returnDTO);
        Task Update(ReturnDTO returnDTO);
        Task Delete(int id);
    }
}
