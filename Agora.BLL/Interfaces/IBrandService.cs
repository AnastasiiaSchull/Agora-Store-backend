using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDTO>> GetAll();
        Task<BrandDTO> Get(int id);
        Task<IEnumerable<BrandDTO>> GetBySubcategory(int subcategoryId);
        Task Create(BrandDTO brandDTO);
        Task Update(BrandDTO brandDTO);
        Task Delete(int id);
    }
}
