using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<SubcategoryDTO>> GetAll();
        Task<SubcategoryDTO> Get(int id);
        Task Create(SubcategoryDTO subcategoryDTO);
        Task Update(SubcategoryDTO subcategoryDTO);
        Task Delete(int id);
    }
}
