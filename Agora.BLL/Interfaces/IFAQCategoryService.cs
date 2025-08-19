using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IFAQCategoryService
    {
        Task<IEnumerable<FAQCategoryDTO>> GetAll();
        Task<FAQCategoryDTO> Get(int id);
        Task Create(FAQCategoryDTO faqCategoryDTO);
        Task Update(FAQCategoryDTO faqCategoryDTO);
        Task Delete(int id);
    }
}
