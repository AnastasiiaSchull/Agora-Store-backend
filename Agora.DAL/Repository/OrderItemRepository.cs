using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class OrderItemRepository: IOrderItemRepository
    {
        private AgoraContext db;
        public OrderItemRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<OrderItem>> GetAll()
        {
            return db.OrderItems;
        }
        public async Task<IQueryable<OrderItem>> GetNewOrders(int storeId)
        {
            return db.OrderItems.Where(o => o.Product.Store.Id == storeId && o.Status == Enums.OrderStatus.Pending);
        }
        public async Task<IQueryable<OrderItem>> GetAllByStore(int storeId)
        {
            return db.OrderItems.Where(o => o.Product.Store.Id == storeId).OrderByDescending(o => o.Date);
        }
        public async Task<OrderItem> Get(int id)
        {
            return await db.OrderItems.FindAsync(id);
        }

        public async Task Create(OrderItem orderItem)
        {
            await db.OrderItems.AddAsync(orderItem);
        }

        public void Update(OrderItem orderItem)
        {
            db.Entry(orderItem).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            OrderItem? orderItem = await db.OrderItems.FindAsync(id);
            if (orderItem != null)
                db.OrderItems.Remove(orderItem);
        }
    }
}
