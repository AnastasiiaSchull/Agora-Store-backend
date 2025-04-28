using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Agora.BLL.Infrastructure;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Services
{
    public class BankCardService : IBankCardService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public BankCardService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<BankCardDTO>> GetAll()
        {
            var bankCards = await Database.BankCards.GetAll();
            return _mapper.Map<IQueryable<BankCardDTO>>(bankCards.ToList());
        }

        public async Task<BankCardDTO> Get(int id)
        {
            var bankCard = await Database.BankCards.Get(id);
            if (bankCard == null)
                throw new ValidationExceptionFromService("There is no bank card with this id", "");
            return new BankCardDTO
            {
                Id = bankCard.Id,
                Number = bankCard.Number,
                Holder = bankCard.Holder,
                ExpirationDate = bankCard.ExpirationDate,
                CustomerId = bankCard.Customer.Id,
                PaymentMethodId = bankCard.PaymentMethod.Id,
            };
        }

        public async Task Create(BankCardDTO bankCardDTO)
        {
            var bankCard = new BankCard
            {
                Id = bankCardDTO.Id,
                Number= bankCardDTO.Number,
                Holder = bankCardDTO.Holder,
                ExpirationDate = bankCardDTO.ExpirationDate,
            };
            await Database.BankCards.Create(bankCard);
            await Database.Save();
        }
        public async Task Update(BankCardDTO bankCardDTO)
        {
            var bankCard = new BankCard
            {
                Id = bankCardDTO.Id,
                Number = bankCardDTO.Number,
                Holder = bankCardDTO.Holder,
                ExpirationDate = bankCardDTO.ExpirationDate,
            };
            Database.BankCards.Update(bankCard);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.BankCards.Delete(id);
            await Database.Save();
        }
    }
}
