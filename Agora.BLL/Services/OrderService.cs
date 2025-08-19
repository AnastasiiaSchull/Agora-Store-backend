using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;

namespace Agora.BLL.Services
{
    public class OrderService : IOrderService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public OrderService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<OrderDTO>> GetAll()
        {
            var orders = await Database.Orders.GetAll();
            return _mapper.Map<IQueryable<OrderDTO>>(orders.ToList());

        }
        //public async Task<IEnumerable<OrderDTO>> GetFilteredBy(string filter) //????
        //{

        //    var filteredOrders = await Database.Orders.Find(s => s);
        //    return _mapper.Map<IEnumerable<OrderDTO>>(filteredOrders);
        //}
        public async Task<OrderDTO> Get(int id)
        {
            var order = await Database.Orders.Get(id);
            if (order == null)
                throw new ValidationExceptionFromService("There is no product with this id", "");
            return new OrderDTO
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                PaymentDeadline = order.PaymentDeadline
              
            };
        }

        public async Task<int> Create(OrderDTO orderDTO)
        {
            try
            {
                var order = new Order
                {
                    TotalPrice = orderDTO.TotalPrice,
                    PaymentDeadline = orderDTO.PaymentDeadline,
                    CustomerId = orderDTO.CustomerId
                };
                await Database.Orders.Create(order);
                await Database.Save();
                return order.Id;
            }
            catch (Exception ex) {
                throw new ValidationExceptionFromService("Error while creating order", ex.Message);
            }
          
        }
        public async Task Update(OrderDTO orderDTO)
        {
            var order = new Order
            {
                Id = orderDTO.Id,
                TotalPrice = orderDTO.TotalPrice,
                
                PaymentDeadline = orderDTO.PaymentDeadline

            };
            Database.Orders.Update(order);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Orders.Delete(id);
            await Database.Save();
        }
    }
}
