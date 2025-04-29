using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IFAQCategoryService
    {
        Task<IQueryable<FAQCategoryDTO>> GetAll();
        Task<FAQCategoryDTO> Get(int id);
        Task Create(FAQCategoryDTO faqCategoryDTO);
        Task Update(FAQCategoryDTO faqCategoryDTO);
        Task Delete(int id);
    }
}
