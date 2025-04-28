using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IAdminService
    {
        Task<IQueryable<AdminDTO>> GetAll();
        Task<AdminDTO> Get(int id);
        Task Create(AdminDTO adminDTO);
        Task Update(AdminDTO adminDTO);
        Task Delete(int id);
    }
}
