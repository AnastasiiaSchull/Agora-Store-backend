using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAll();
        Task<CategoryDTO> Get(int id);
        Task Create(CategoryDTO categoryDTO);
        Task Update(CategoryDTO categoryDTO);
        Task Delete(int id);
    }
}
