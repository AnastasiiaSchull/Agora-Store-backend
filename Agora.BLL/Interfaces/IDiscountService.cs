using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IDiscountService
    {
        Task<IQueryable<DiscountDTO>> GetAll();
        Task<DiscountDTO> Get(int id);
        Task Create(DiscountDTO discountDTO);
        Task Update(DiscountDTO discountDTO);
        Task Delete(int id);
    }
}
