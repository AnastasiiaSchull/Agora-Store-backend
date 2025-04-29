using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface ICustomerService
    {
        Task<IQueryable<CustomerDTO>> GetAll();
        Task<CustomerDTO> Get(int id);
        Task Create(CustomerDTO customerDTO);
        Task Update(CustomerDTO customerDTO);
        Task Delete(int id);
        Task<bool> CreateForUser(int userId);
    }
}
