using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IAddressUserRepository
    {
        Task Create(AddressUser entity);
    }
}
