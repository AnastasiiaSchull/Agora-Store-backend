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
using Agora.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace Agora.BLL.Services
{
    public class OrderItemService : IOrderItemService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public OrderItemService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<OrderItemDTO>> GetAll()
        {
            var orderItem = await Database.OrderItems.GetAll();
            return _mapper.Map<IQueryable<OrderItemDTO>>(orderItem.ToList());

        }
        public async Task<List<OrderItemDTO>> GetNewOrders(int storeId)
        {
            var orderItems = await Database.OrderItems.GetNewOrders(storeId);
            return _mapper.Map<List<OrderItemDTO>>(orderItems.ToList());

        }
        public async Task<List<OrderItemDTO>> GetAllByStore(int storeId)
        {
            var orderItem = await Database.OrderItems.GetAllByStore(storeId);
            return _mapper.Map<List<OrderItemDTO>>(orderItem.ToList());

        }
        public async Task<List<OrderItemDTO>> GetFiltredOrders(int storeId, string field, string value)
        {
          
            var orderItem = await Database.OrderItems.GetFiltredOrders(storeId, field, value);
            return _mapper.Map<List<OrderItemDTO>>(orderItem.ToList());
           
        }
        public async Task<OrderItemDTO> Get(int id)
        {
            var order = await Database.OrderItems.Get(id);
            if (order == null)
                throw new ValidationExceptionFromService("There is no order with this id", "");
            return new OrderItemDTO
            {
                Id = order.Id,
                PriceAtMoment = order.PriceAtMoment,
                Quantity = order.Quantity
               
            };
        }

        public async Task Create(OrderItemDTO orderItemDTO)
        {
            var orderItem = new OrderItem
            {
                PriceAtMoment = orderItemDTO.PriceAtMoment,
                Quantity = orderItemDTO.Quantity
            };
            await Database.OrderItems.Create(orderItem);
            await Database.Save();
        }
        public async Task Update(OrderItemDTO orderItemDTO)
        {
            var orderItem = new OrderItem
            {
                Id = orderItemDTO.Id,
                PriceAtMoment = orderItemDTO.PriceAtMoment,
                Quantity = orderItemDTO.Quantity
            };
            Database.OrderItems.Update(orderItem);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.OrderItems.Delete(id);
            await Database.Save();
        }
    }
}
