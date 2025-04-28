using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Agora.DAL.Repository
{
    public class OrderRepository : IRepository<Order>
    {
        private AgoraContext db;
        public OrderRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Order>> GetAll()
        {
            return db.Orders;
        }

        public async Task<Order> Get(int id)
        {
            return await db.Orders.FindAsync(id);
        }

        public async Task Create(Order order)
        {
            await db.Orders.AddAsync(order);
        }

        public void Update(Order order)
        {
            db.Entry(order).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Order? order = await db.Orders.FindAsync(id);
            if (order != null)
                db.Orders.Remove(order);
        }

        public async Task<IEnumerable<Order>> Find(Expression<Func<Order, bool>> predicate)
        {
            return await db.Orders.Where(predicate).ToListAsync();
        }
    }
}