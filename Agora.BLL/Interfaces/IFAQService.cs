using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IFAQService
    {
        Task<IQueryable<FAQDTO>> GetAll();
        Task<FAQDTO> Get(int id);
        Task Create(FAQDTO faqDTO);
        Task Update(FAQDTO faqDTO);
        Task Delete(int id);
    }
}
