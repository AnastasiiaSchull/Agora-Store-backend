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
    public class CustomerService : ICustomerService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public CustomerService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<CustomerDTO>> GetAll()
        {
            var customers = await Database.Customers.GetAll();
            return _mapper.Map<IQueryable<CustomerDTO>>(customers.ToList());
        }

        public async Task<CustomerDTO> Get(int id)
        {
            var customer = await Database.Customers.Get(id);
            if (customer == null)
                throw new ValidationExceptionFromService("There is no customer with this id", "");
            return new CustomerDTO
            {
                Id = customer.Id,
                UserId = customer.UserId,
                CashbackId = customer.Cashback.Id,
            };
        }

        public async Task Create(CustomerDTO customerDTO)
        {
            var customer = new Customer
            {
                Id = customerDTO.Id,
                UserId= customerDTO.UserId,
            };
            await Database.Customers.Create(customer);
            await Database.Save();
        }
        public async Task Update(CustomerDTO customerDTO)
        {
            var customer = new Customer
            {
                Id = customerDTO.Id,
                UserId = customerDTO.UserId,
            };
            Database.Customers.Update(customer);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Customers.Delete(id);
            await Database.Save();
        }

        public async Task<bool> CreateForUser(int userId)
        {
            try
            {
                var customer = new Customer
                {
                    UserId = userId,
                };

                await Database.Customers.Create(customer);
                await Database.Save();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
