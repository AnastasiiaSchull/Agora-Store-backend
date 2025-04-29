using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class DeliveryOptionsRepository : IRepository<DeliveryOptions>
    {
        private AgoraContext db;
        public DeliveryOptionsRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<DeliveryOptions>> GetAll()
        {
            return db.DeliveryOptions;
        }

        public async Task<DeliveryOptions> Get(int id)
        {
            return await db.DeliveryOptions.FindAsync(id);
        }

        public async Task Create(DeliveryOptions deliveryOptions)
        {
            await db.DeliveryOptions.AddAsync(deliveryOptions);
        }

        public void Update(DeliveryOptions deliveryOptions)
        {
            db.Entry(deliveryOptions).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            DeliveryOptions? deliveryOptions = await db.DeliveryOptions.FindAsync(id);
            if (deliveryOptions != null)
                db.DeliveryOptions.Remove(deliveryOptions);
        }
    }
}