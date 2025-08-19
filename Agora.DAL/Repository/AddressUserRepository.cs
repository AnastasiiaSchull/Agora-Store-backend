using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class AddressUserRepository : IAddressUserRepository
    {
        private AgoraContext _context;

        public AddressUserRepository (AgoraContext context)
        {
            _context = context;
        }

        public async Task Create(AddressUser entity)
        {
            await _context.AddressUser.AddAsync(entity);
        }

        public async Task Delete(int addressId)
        {
            var addressUsers = await _context.AddressUser
                .Where(au => au.AddressesId == addressId)
                .ToListAsync();
            
            if (addressUsers.Any())
                _context.AddressUser.RemoveRange(addressUsers);
        }
    }
}
