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
    public class CashbackService : ICashbackService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public CashbackService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<CashbackDTO>> GetAll()
        {
            var cashbacks = await Database.Cashbacks.GetAll();
            return _mapper.Map<IQueryable<CashbackDTO>>(cashbacks.ToList());
        }

        public async Task<CashbackDTO> Get(int id)
        {
            var cashback = await Database.Cashbacks.Get(id);
            if (cashback == null)
                throw new ValidationExceptionFromService("There is no cashback with this id", "");
            return new CashbackDTO
            {
                Id = cashback.Id,
                Amount = cashback.Amount,
                CustomerId = cashback.CustomerId,
                PaymentMethodId = cashback.PaymentMethod.Id,
            };
        }

        public async Task Create(CashbackDTO cashbackDTO)
        {
            var cashback = new Cashback
            {
                Id = cashbackDTO.Id,
                Amount = cashbackDTO.Amount,
                CustomerId= cashbackDTO.CustomerId,
            };
            await Database.Cashbacks.Create(cashback);
            await Database.Save();
        }
        public async Task Update(CashbackDTO cashbackDTO)
        {
            var cashback = new Cashback
            {
                Id = cashbackDTO.Id,
                Amount = cashbackDTO.Amount,
                CustomerId = cashbackDTO.CustomerId,
            };
            Database.Cashbacks.Update(cashback);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Cashbacks.Delete(id);
            await Database.Save();
        }
    }
}
