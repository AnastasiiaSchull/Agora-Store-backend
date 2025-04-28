using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IWishlistService
    { 
        Task<IQueryable<WishlistDTO>> GetAll();
        Task<WishlistDTO> Get(int id);
        Task Create(WishlistDTO wishlistDTO);
        Task Update(WishlistDTO wishlistDTO);
        Task Delete(int id);
    }
}
