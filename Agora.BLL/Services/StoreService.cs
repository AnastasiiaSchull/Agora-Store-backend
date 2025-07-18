using Agora.BLL.Interfaces;
using Agora.BLL.Infrastructure;
using Agora.DAL.Interfaces;
using AutoMapper;
using Agora.BLL.DTO;
using Agora.DAL.Entities;
using Agora.Enums;

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
                SellerId = store.SellerId,
                FundsBalance = store.FundsBalance
            };
        }
        public async Task<int> Create(StoreDTO storeDTO)
        {        
            var store = new Store
            {
                Name = storeDTO.Name,
                Description = storeDTO.Description,
                CreatedAt = storeDTO.CreatedAt,
                UpdatedAt = storeDTO.UpdatedAt,
                SellerId = storeDTO.SellerId,
                FundsBalance = storeDTO.FundsBalance
            };
            await Database.Stores.Create(store);
            await Database.Save();

            return store.Id;
        }
        public async Task Update(StoreDTO storeDTO)
        {
            var existingStore = await Database.Stores.Get(storeDTO.Id);

            if (existingStore == null)
                throw new Exception($"Store with ID {storeDTO.Id} not found");

            if (!string.IsNullOrWhiteSpace(storeDTO.Name))
                existingStore.Name = storeDTO.Name;

            if (!string.IsNullOrWhiteSpace(storeDTO.Description))
                existingStore.Description = storeDTO.Description;

            if (storeDTO.FundsBalance != null)
                existingStore.FundsBalance = storeDTO.FundsBalance;

            if (storeDTO.CreatedAt != default)
                existingStore.CreatedAt = storeDTO.CreatedAt;

            if (storeDTO.UpdatedAt != default)
                existingStore.UpdatedAt = storeDTO.UpdatedAt;

            if (storeDTO.SellerId.HasValue)
                existingStore.SellerId = storeDTO.SellerId.Value;


            Database.Stores.Update(existingStore);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Stores.Delete(id);
            await Database.Save();
        }
    }
}
