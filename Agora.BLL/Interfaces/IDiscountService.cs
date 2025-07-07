using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountDTO>> GetAll();
        Task<IEnumerable<DiscountDTO>> GetActiveDiscounts();
        Task<DiscountDTO> Get(int id);
        Task Create(DiscountDTO discountDTO);
        Task Update(DiscountDTO discountDTO);
        Task Delete(int id);
    }
}
