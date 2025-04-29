using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IAddressService
    {
        Task<IQueryable<AddressDTO>> GetAll();
        Task<AddressDTO> Get(int id);
        Task Create(AddressDTO addressDTO);
        Task Update(AddressDTO addressDTO);
        Task Delete(int id);
    }
}
