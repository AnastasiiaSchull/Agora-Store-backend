using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class StoreRepository: IStoreRepository
    {
        private AgoraContext db;
        public StoreRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Store>> GetAll()
        {
            return db.Stores;
        }
        
        public async Task<Store> Get(int id)
        {
            return await db.Stores.FindAsync(id);
        }

        public async Task Create(Store store)
        {
            await db.Stores.AddAsync(store);
        }

        public void Update(Store store)
        {
            db.Entry(store).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Store? store = await db.Stores.FindAsync(id);
            if (store != null)
                db.Stores.Remove(store);
        }
    }
}
