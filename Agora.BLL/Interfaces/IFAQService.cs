using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IFAQService
    {
        Task<IEnumerable<FAQDTO>> GetAll();
        Task<FAQDTO> Get(int id);
        Task Create(FAQDTO faqDTO);
        Task Update(FAQDTO faqDTO);
        Task Delete(int id);
    }
}
