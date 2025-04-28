using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class ReturnItemRepository: IRepository<ReturnItem>
    {
        private AgoraContext db;
        public ReturnItemRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<ReturnItem>> GetAll()
        {
            return db.ReturnItems;
        }

        public async Task<ReturnItem> Get(int id)
        {
            return await db.ReturnItems.FindAsync(id);
        }

        public async Task Create(ReturnItem returnItem)
        {
            await db.ReturnItems.AddAsync(returnItem);
        }

        public void Update(ReturnItem returnItem)
        {
            db.Entry(returnItem).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            ReturnItem? returnItem = await db.ReturnItems.FindAsync(id);
            if (returnItem != null)
                db.ReturnItems.Remove(returnItem);
        }
    }
}
