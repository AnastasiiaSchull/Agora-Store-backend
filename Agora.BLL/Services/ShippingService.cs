using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.BLL.Infrastructure;
using Agora.DAL.Interfaces;
using AutoMapper;
using Agora.DAL.Entities;
using Agora.Enums;
using StackExchange.Redis;

namespace Agora.BLL.Services
{
    public class ShippingService : IShippingService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public ShippingService(IUnitOfWork database, IMapper mapper)
        {
            Database = database;
            _mapper = mapper;
        }
        public async Task<IQueryable<ShippingDTO>> GetAll()
        {
            var shippings = await Database.Shippings.GetAll();
            return _mapper.Map<IQueryable<ShippingDTO>>(shippings.ToList());
        }
        public async Task<ShippingDTO> Get(int id)
        {
            var shipping = await Database.Shippings.Get(id);
            if (shipping == null)
                throw new ValidationExceptionFromService("There is no shipping with this id", "");
            return new ShippingDTO
            {
                Id = shipping.Id,
                Status = shipping.Status.ToString(),
                TrackingNumber = shipping.TrackingNumber,
                OrderItemId = shipping.OrderItemId,
                DeliveryOptionsId = shipping.DeliveryOptionsId
            };
        }
        public async Task<ShippingDTO> GetByOrderItem(int id)
        {
            var shipping = await Database.Shippings.GetByOrderItem(id);
            if (shipping == null)
                throw new ValidationExceptionFromService("There is no shipping with this id", "");
            return new ShippingDTO
            {
                Id = shipping.Id,
                Status = shipping.Status.ToString(),
                TrackingNumber = shipping.TrackingNumber,
                OrderItemId = shipping.OrderItemId,
                DeliveryOptionsId = shipping.DeliveryOptionsId,
                //OrderItemDTO = _mapper.Map<OrderItemDTO>(shipping.OrderItem),
                DeliveryOptionsDTO = _mapper.Map<DeliveryOptionsDTO>(shipping.DeliveryOptions),
                AddressDTO = _mapper.Map<AddressDTO>(shipping.Address)
            };
        }
        public async Task Create(ShippingDTO shippingDTO)
        {
            var shipping = new Shipping
            {
                Status = Enum.Parse<ShippingStatus>(shippingDTO.Status, ignoreCase: true),
                TrackingNumber = shippingDTO.TrackingNumber,
                OrderItemId = shippingDTO.OrderItemId,
                DeliveryOptionsId = shippingDTO.DeliveryOptionsId
            };
            await Database.Shippings.Create(shipping);
            await Database.Save();
        }
        public async Task Update(ShippingDTO shippingDTO)
        {
            var existingShipping = await Database.Shippings.Get(shippingDTO.Id);
            if (!string.IsNullOrWhiteSpace(shippingDTO.Status))
                existingShipping.Status = Enum.Parse<ShippingStatus>(shippingDTO.Status, true);

            if (!string.IsNullOrWhiteSpace(shippingDTO.TrackingNumber))
                existingShipping.TrackingNumber = shippingDTO.TrackingNumber;

            if (shippingDTO.OrderItemId.HasValue)
                existingShipping.OrderItemId = shippingDTO.OrderItemId.Value;

            if (shippingDTO.AddressId.HasValue)
                existingShipping.AddressId = shippingDTO.AddressId.Value;

            if (shippingDTO.DeliveryOptionsId.HasValue)
                existingShipping.DeliveryOptionsId = shippingDTO.DeliveryOptionsId.Value;
           
            await Database.Shippings.Update(existingShipping);
            await Database.Save();
        }
        public async Task Delete(int id)
        {
            await Database.Shippings.Delete(id);
            await Database.Save();
        }
    }
}
