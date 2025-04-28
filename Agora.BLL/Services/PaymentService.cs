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
    public class PaymentService : IPaymentService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public PaymentService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<PaymentDTO>> GetAll()
        {
            var payments = await Database.Payments.GetAll();
            return _mapper.Map<IQueryable<PaymentDTO>>(payments.ToList());

        }
        public async Task<PaymentDTO> Get(int id)
        {
            var payment = await Database.Payments.Get(id);
            if (payment == null)
                throw new ValidationExceptionFromService("There is no user with this id", "");
            return new PaymentDTO
            {
                Id = payment.Id,
                Amount = payment.Amount,
                TransactionDate = payment.TransactionDate,
                CashbackUsed = payment.CashbackUsed,
                Status = payment.Status,
                PaymentMethodId = payment.PaymentMethodId,
                OrderId = payment.OrderId

            };
        }

        public async Task Create(PaymentDTO paymentDTO)
        {
            var payment = new Payment
            {
               
                Amount = paymentDTO.Amount,
                TransactionDate = paymentDTO.TransactionDate,
                CashbackUsed = paymentDTO.CashbackUsed,
                Status = paymentDTO.Status,
                PaymentMethodId = paymentDTO.PaymentMethodId,
                OrderId = paymentDTO.OrderId

            };
            await Database.Payments.Create(payment);
            await Database.Save();
        }
        public async Task Update(PaymentDTO paymentDTO)
        {
            var payment = new Payment
            {
                Id = paymentDTO.Id,
                Amount = paymentDTO.Amount,
                TransactionDate = paymentDTO.TransactionDate,
                CashbackUsed = paymentDTO.CashbackUsed,
                Status = paymentDTO.Status,
                PaymentMethodId = paymentDTO.PaymentMethodId,
                OrderId = paymentDTO.OrderId

            };
            Database.Payments.Update(payment);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Payments.Delete(id);
            await Database.Save();
        }
    }
}
