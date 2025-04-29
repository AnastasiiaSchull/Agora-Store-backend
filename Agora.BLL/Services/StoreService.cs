using Agora.BLL.Interfaces;
using Agora.BLL.Infrastructure;
using Agora.DAL.Interfaces;
using AutoMapper;
using Agora.BLL.DTO;
using Agora.DAL.Entities;

namespace Agora.BLL.Services
{
    public class StoreService : IStoreService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;        
        public StoreService(IUnitOfWork database, IMapper mapper)
        {
            Database = database;
            _mapper = mapper;           
        }
        
        public async Task<IEnumerable<StoreDTO>> GetAll()
        {
            var stores = await Database.Stores.GetAll();
            return _mapper.Map<IQueryable<Store>, IEnumerable<StoreDTO>>(stores);
        }

        // Ids of all stores for Redis:
        public async Task<List<int>> GetAllStoreIds()
        {
            var stores = await Database.Stores.GetAll();
            return stores.Select(s => s.Id).ToList();
        }
        

        public async Task<StoreDTO> Get(int id)
        {
            var store = await Database.Stores.Get(id);
            if (store == null)
                throw new ValidationExceptionFromService("There is no store with this id", "");
            return new StoreDTO
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                CreatedAt = store.CreatedAt,
                UpdatedAt = store.UpdatedAt,
                SellerId = store.SellerId
            };
        }
        public async Task Create(StoreDTO storeDTO)
        {        
            var store = new Store
            {
                Name = storeDTO.Name,
                Description = storeDTO.Description,
                CreatedAt = storeDTO.CreatedAt,
                UpdatedAt = storeDTO.UpdatedAt,
                SellerId = storeDTO.SellerId
            };
            await Database.Stores.Create(store);
            await Database.Save();
        }
        public async Task Update(StoreDTO storeDTO)
        {
            var store = new Store
            {
                Id = storeDTO.Id,
                Name = storeDTO.Name,
                Description = storeDTO.Description,
                CreatedAt = storeDTO.CreatedAt,
                UpdatedAt = storeDTO.UpdatedAt,
                SellerId = storeDTO.SellerId
            };
            Database.Stores.Update(store);
            await Database.Save();
        }
        public async Task Delete(int id)
        {
            await Database.Stores.Delete(id);
            await Database.Save();
        }
    }
}
