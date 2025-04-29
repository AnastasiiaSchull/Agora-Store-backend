using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class AddressRepository : IRepository<Address>
    {
        private AgoraContext db;
        public AddressRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Address>> GetAll()
        {
            return db.Addresses;
        }

        public async Task<Address> Get(int id)
        {
            return await db.Addresses.FindAsync(id);
        }

        public async Task Create(Address address)
        {
            await db.Addresses.AddAsync(address);
        }

        public void Update(Address address)
        {
            db.Entry(address).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Address? аddress = await db.Addresses.FindAsync(id);
            if (аddress != null)
                db.Addresses.Remove(аddress);
        }
    }
}
