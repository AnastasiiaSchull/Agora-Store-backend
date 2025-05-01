using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<IQueryable<OrderItem>> GetAll();
        Task<IQueryable<OrderItem>> GetNewOrders(int storeId);
        Task<IQueryable<OrderItem>> GetAllByStore(int storeId);
        Task<IEnumerable<OrderItem>> GetFiltredOrders(int storeId, string field, string value);
        Task<OrderItem> Get(int id);
        Task Create(OrderItem item);
        void Update(OrderItem item);
        Task Delete(int id);

    }
}
