using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private AgoraContext db;
        public AddressRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Address>> GetAll()
        {
            return db.Addresses.Include(a => a.Country);
        }

        public async Task<Address> Get(int id)
        {
            return await db.Addresses
                   .Include(a => a.Country)
                   .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Address>> GetWithCountryByUserId(int userId)
        {
            return await db.Addresses
                .Include(a => a.Country)
                .Where(a => a.AddressUsers.Any(au => au.UserId == userId))
                .ToListAsync();
        }

        public async Task<Address> GetAddressByUserIdAsync(int userId)
        {
            return await db.AddressUser
                .Where(au => au.UserId == userId)
                .Select(au => au.Address)
                .FirstOrDefaultAsync();
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
