using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IAddressService
    {
        Task<IQueryable<AddressDTO>> GetAll();
        Task<AddressDTO> Get(int id);
        Task<IEnumerable<AddressDTO>> GetByUserId(int userId);
        Task Create(AddressDTO addressDTO);
        Task Update(AddressDTO addressDTO);
        Task UpdateSellerAddressAsync(UpdateSellerAddressDTO dto);
        Task Delete(int id);
        Task CreateAddress(AddressDTO dto);
        Task UpdateAddress(AddressDTO dto, int id);
        Task DeleteAddress(int id);
        Task<AddressDTO> GetByUserIdAsync(int userId);
    }
}
