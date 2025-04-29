using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private AgoraContext db;
        public CustomerRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Customer>> GetAll()
        {
            return db.Customers;
        }

        public async Task<Customer> Get(int id)
        {
            return await db.Customers.FindAsync(id);
        }
        public async Task<Customer> GetByUserId(int id)
        {
            return await db.Customers.FirstOrDefaultAsync(a => a.UserId == id);
        }
        public async Task Create(Customer customer)
        {
            await db.Customers.AddAsync(customer);
        }

        public void Update(Customer customer)
        {
            db.Entry(customer).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Customer? customer = await db.Customers.FindAsync(id);
            if (customer != null)
                db.Customers.Remove(customer);
        }
    }
}