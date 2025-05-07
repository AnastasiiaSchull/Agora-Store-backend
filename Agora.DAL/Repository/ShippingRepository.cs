using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class ShippingRepository: IShippingRepository
    {
        private AgoraContext db;
        public ShippingRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Shipping>> GetAll()
        {
            return db.Shippings;
        }

        public async Task<Shipping> Get(int id)
        {
            return await db.Shippings.FindAsync(id);
        }
        public async Task<Shipping> GetByOrderItem(int id)
        {
            return await db.Shippings.FirstOrDefaultAsync(s => s.OrderItemId == id);
        }

        public async Task Create(Shipping shipping)
        {
            await db.Shippings.AddAsync(shipping);
        }

        public void Update(Shipping shipping)
        {
            db.Entry(shipping).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Shipping? shipping = await db.Shippings.FindAsync(id);
            if (shipping != null)
                db.Shippings.Remove(shipping);
        }
    }
}
