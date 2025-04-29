using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreDTO>> GetAll();
        Task<List<int>> GetAllStoreIds();
      
        Task<StoreDTO> Get(int id);
        Task Create(StoreDTO storeDTO);
        Task Update(StoreDTO storeDTO);
        Task Delete(int id);
    }
}
