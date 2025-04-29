using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IGiftCardService
    {
        Task<IQueryable<GiftCardDTO>> GetAll();
        Task<GiftCardDTO> Get(int id);
        Task Create(GiftCardDTO giftCardDTO);
        Task Update(GiftCardDTO giftCardDTO);
        Task Delete(int id);
    }
}
