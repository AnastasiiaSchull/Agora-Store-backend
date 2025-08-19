using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        //Task<Address> GetWithDetails(int id);
        //Task<IQueryable<Address>> GetAllWithDetails();
        //Task<IEnumerable<Address>> GetByUserIdWithDetails(int userId);
        Task<IEnumerable<Address>> GetWithCountryByUserId(int userId);
        Task<Address> GetAddressByUserIdAsync(int userId);
    }
}
