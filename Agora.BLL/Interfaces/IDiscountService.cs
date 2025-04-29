using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
