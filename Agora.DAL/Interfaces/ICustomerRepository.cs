using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IQueryable<Customer>> GetAll();
        Task<Customer> GetByUserId(int id);
        Task<Customer> Get(int id);
        Task Create(Customer customer);
        void Update(Customer customer);
        Task Delete(int id);
    }
}
