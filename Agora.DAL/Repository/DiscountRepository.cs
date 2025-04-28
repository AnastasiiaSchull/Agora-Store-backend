using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class DiscountRepository : IRepository<Discount>
    {
        private AgoraContext db;
        public DiscountRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Discount>> GetAll()
        {
            return db.Discounts;
        }

        public async Task<Discount> Get(int id)
        {
            return await db.Discounts.FindAsync(id);
        }

        public async Task Create(Discount discount)
        {
            await db.Discounts.AddAsync(discount);
        }

        public void Update(Discount discount)
        {
            db.Entry(discount).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Discount? discount = await db.Discounts.FindAsync(id);
            if (discount != null)
                db.Discounts.Remove(discount);
        }
    }
}