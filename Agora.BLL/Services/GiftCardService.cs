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
    public class GiftCardService : IGiftCardService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public GiftCardService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<GiftCardDTO>> GetAll()
        {
            var giftCard = await Database.GiftCards.GetAll();
            return _mapper.Map<IQueryable<GiftCardDTO>>(giftCard.ToList());
        }

        public async Task<GiftCardDTO> Get(int id)
        {
            var giftCard = await Database.GiftCards.Get(id);
            if (giftCard == null)
                throw new ValidationExceptionFromService("There is no gift card with this id", "");
            return new GiftCardDTO
            {
                Id = giftCard.Id,
                Code = giftCard.Code,
                Balance = giftCard.Balance,
                ExpirationDate = giftCard.ExpirationDate,
                CustomerId = giftCard.Customer.Id,
                PaymentMethodId = giftCard.PaymentMethod.Id,
            };
        }

        public async Task Create(GiftCardDTO giftCardDTO)
        {
            var giftCard = new GiftCard
            {
                Id = giftCardDTO.Id,
                Code = giftCardDTO.Code,
                Balance= giftCardDTO.Balance,
                ExpirationDate= giftCardDTO.ExpirationDate,
            };
            await Database.GiftCards.Create(giftCard);
            await Database.Save();
        }
        public async Task Update(GiftCardDTO giftCardDTO)
        {
            var giftCard = new GiftCard
            {
                Id = giftCardDTO.Id,
                Code = giftCardDTO.Code,
                Balance = giftCardDTO.Balance,
                ExpirationDate = giftCardDTO.ExpirationDate,
            };
            Database.GiftCards.Update(giftCard);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.GiftCards.Delete(id);
            await Database.Save();
        }
    }
}
