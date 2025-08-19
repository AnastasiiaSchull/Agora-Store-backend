using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface ISupportService
    {
        Task<IQueryable<SupportDTO>> GetAll();
        Task<SupportDTO> Get(int id);
        Task Create(SupportDTO supportDTO);
        Task Update(SupportDTO supportDTO);
        Task Delete(int id);
    }
}
