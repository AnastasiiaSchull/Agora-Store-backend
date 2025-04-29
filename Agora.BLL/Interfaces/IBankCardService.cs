using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IBankCardService
    {
        Task<IQueryable<BankCardDTO>> GetAll();
        Task<BankCardDTO> Get(int id);
        Task Create(BankCardDTO bankCardDTO);
        Task Update(BankCardDTO bankCardDTO);
        Task Delete(int id);
    }
}
