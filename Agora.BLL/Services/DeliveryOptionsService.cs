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
using Agora.Enums;

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
                Type = deliveryOptions.Type.ToString(),
                Price = deliveryOptions.Price,
                EstimatedDays = deliveryOptions.EstimatedDays,
                ShippingId = deliveryOptions.Shipping?.Id,
            };
        }

        public async Task Create(DeliveryOptionsDTO deliveryOptionsDTO)
        {
            var deliveryOptions = new DeliveryOptions
            {
                Id = deliveryOptionsDTO.Id,
                Type= Enum.Parse<DeliveryType>(deliveryOptionsDTO.Type, ignoreCase: true),
                Price = deliveryOptionsDTO.Price,
                EstimatedDays= deliveryOptionsDTO.EstimatedDays,
                SellerId = deliveryOptionsDTO.SellerId
            };

            await Database.DeliveryOptions.Create(deliveryOptions);
            await Database.Save();
        }

        public async Task Update(int id, DeliveryOptionsDTO deliveryOptionsDTO)
        {
            var deliveryOptions = new DeliveryOptions
            {
                Id = id,
                Type = Enum.Parse<DeliveryType>(deliveryOptionsDTO.Type, ignoreCase: true),
                Price = deliveryOptionsDTO.Price,
                EstimatedDays = deliveryOptionsDTO.EstimatedDays,
                SellerId = deliveryOptionsDTO.SellerId
            };

            Database.DeliveryOptions.Update(deliveryOptions);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.DeliveryOptions.Delete(id);
            await Database.Save();
        }

        public async Task<IEnumerable<DeliveryOptionsDTO>> GetBySellerId(int sellerId)
        {
            var deliveryOptions = await Database.DeliveryOptions.GetAll();
            var filtered = deliveryOptions
                .Where(opt => opt.Seller != null && opt.Seller.Id == sellerId)
                .Select(opt => new DeliveryOptionsDTO
                {
                    Id = opt.Id,
                    Type = opt.Type.ToString(),
                    Price = opt.Price,
                    EstimatedDays = opt.EstimatedDays,
                    ShippingId = opt.Shipping.Id
                });

            Console.WriteLine($"Filtered count: {filtered.Count()}");

            foreach (var item in filtered)
            {
                Console.WriteLine($"Filtered Option: {item.Id}, Type: {item.Type}, Price: {item.Price}, SellerId: {sellerId}");
            }

            return filtered;
        }

        public async Task DeleteAllBySellerId(int sellerId)
        {
            var options = await Database.DeliveryOptions.GetAll();
            var toDelete = options.Where(opt => opt.Seller != null && opt.Seller.Id == sellerId).ToList();

            foreach (var option in toDelete)
            {
                await Database.DeliveryOptions.Delete(option.Id);
            }

            await Database.Save();
        }
    }
}
