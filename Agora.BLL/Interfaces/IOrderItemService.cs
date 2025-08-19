using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;



namespace Agora.BLL.Interfaces
{
    public interface IOrderItemService
    {
        Task<IQueryable<OrderItemDTO>> GetAll();
        Task<List<OrderItemDTO>> GetNewOrders(int storeId);
        Task<List<OrderItemDTO>> GetAllByStore(int storeId);
        Task<List<OrderItemDTO>> GetAllByCustomer(int customerId, int? months = null);
        Task<List<OrderItemDTO>> GetFiltredOrders(int storeId, string field, string value);
        Task<OrderItemDTO> Get(int id);
        Task<int> Create(OrderItemDTO orderItemDTO);
        Task Update(OrderItemDTO orderItemDTO);
        Task UpdateStatus(int orderItemId, string newStatus);
        Task Delete(int id);
    }
}
