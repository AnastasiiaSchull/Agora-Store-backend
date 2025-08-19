using Agora.BLL.DTO;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDTO>> GetAll();
        Task<BrandDTO> Get(int id);
        Task<IEnumerable<BrandDTO>> GetBySubcategoryOrCategory(int? subcategoryId = null, int? categoryId = null);
        Task Create(BrandDTO brandDTO);
        Task Update(BrandDTO brandDTO);
        Task Delete(int id);
    }
}
