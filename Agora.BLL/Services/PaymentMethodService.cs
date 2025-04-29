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
    public class PaymentMethodService : IPaymentMethodService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public PaymentMethodService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<PaymentMethodDTO>> GetAll()
        {
            var paymentMethods = await Database.PaymentMethods.GetAll();
            return _mapper.Map<IQueryable<PaymentMethodDTO>>(paymentMethods.ToList());

        }
        public async Task<PaymentMethodDTO> Get(int id)
        {
            var paymnetMethod = await Database.PaymentMethods.Get(id);
            if (paymnetMethod == null)
                throw new ValidationExceptionFromService("There is no record with this id", "");
            return new PaymentMethodDTO
            {
                Id = paymnetMethod.Id,
                BankCardId = paymnetMethod.BankCardId,
                GiftCardId = paymnetMethod.GiftCardId,
                CashbackId = paymnetMethod.CashbackId
            };
        }

        public async Task Create(PaymentMethodDTO paymentMethodDTO)
        {
            var paymentMethod = new PaymentMethod
            {
                BankCardId = paymentMethodDTO.BankCardId,
                GiftCardId = paymentMethodDTO.GiftCardId,
                CashbackId = paymentMethodDTO.CashbackId

            };
            await Database.PaymentMethods.Create(paymentMethod);
            await Database.Save();
        }
        public async Task Update(PaymentMethodDTO paymentMethodDTO)
        {
            var paymentMethod = new PaymentMethod
            {
                Id = paymentMethodDTO.Id,
                BankCardId = paymentMethodDTO.BankCardId,
                GiftCardId = paymentMethodDTO.GiftCardId,
                CashbackId = paymentMethodDTO.CashbackId

            };
            Database.PaymentMethods.Update(paymentMethod);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.PaymentMethods.Delete(id);
            await Database.Save();
        }
    }
}
