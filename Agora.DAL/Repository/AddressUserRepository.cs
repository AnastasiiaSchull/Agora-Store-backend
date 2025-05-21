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
    }
}
