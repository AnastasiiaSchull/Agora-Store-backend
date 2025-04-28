using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface ICashbackService
    {
        Task<IQueryable<CashbackDTO>> GetAll();
        Task<CashbackDTO> Get(int id);
        Task Create(CashbackDTO cashbackDTO);
        Task Update(CashbackDTO cashbackDTO);
        Task Delete(int id);
    }
}
