using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreDTO>> GetAll();
        Task<List<int>> GetAllStoreIds();      
        Task<StoreDTO> Get(int id);
        Task<int> Create(StoreDTO storeDTO);
        Task Update(StoreDTO storeDTO);
        Task Delete(int id);
    }
}
