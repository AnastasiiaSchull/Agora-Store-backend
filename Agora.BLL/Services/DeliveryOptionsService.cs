using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;
using Agora.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Services
{
    public class DeliveryOptionsService : IDeliveryOptionsService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public DeliveryOptionsService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<DeliveryOptionsDTO>> GetAll()
        {
            var deliveryOptions = await Database.DeliveryOptions.GetAll();
            return _mapper.Map<IQueryable<DeliveryOptionsDTO>>(deliveryOptions.ToList());
        }

        public async Task<DeliveryOptionsDTO> Get(int id)
        {
            var deliveryOptions = await Database.DeliveryOptions.Get(id);
            if (deliveryOptions == null)
                throw new ValidationExceptionFromService("There is no delivery option with this id", "");
            return new DeliveryOptionsDTO
            {
                Id = deliveryOptions.Id,
                Type = deliveryOptions.Type,
                Price = deliveryOptions.Price,
                EstimatedDays = deliveryOptions.EstimatedDays,
                ShippingId = deliveryOptions.Shipping.Id,
            };
        }

        public async Task Create(DeliveryOptionsDTO deliveryOptionsDTO)
        {
            var deliveryOptions = new DeliveryOptions
            {
                Id = deliveryOptionsDTO.Id,
                Type= deliveryOptionsDTO.Type,
                Price = deliveryOptionsDTO.Price,
                EstimatedDays= deliveryOptionsDTO.EstimatedDays,
            };
            await Database.DeliveryOptions.Create(deliveryOptions);
            await Database.Save();
        }
        public async Task Update(DeliveryOptionsDTO deliveryOptionsDTO)
        {
            var deliveryOptions = new DeliveryOptions
            {
                Id = deliveryOptionsDTO.Id,
                Type = deliveryOptionsDTO.Type,
                Price = deliveryOptionsDTO.Price,
                EstimatedDays = deliveryOptionsDTO.EstimatedDays,
            };
            Database.DeliveryOptions.Update(deliveryOptions);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.DeliveryOptions.Delete(id);
            await Database.Save();
        }
    }
}
