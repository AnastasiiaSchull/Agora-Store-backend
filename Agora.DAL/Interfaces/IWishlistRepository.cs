using Agora.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.DAL.Interfaces
{
    public interface IWishlistRepository
    {
        Task<IQueryable<Wishlist>> GetAll();
        Task<Wishlist> Get(int id);
        Task Create(Wishlist item);
        void Update(Wishlist item);
        Task Delete(int id);
        Task<Wishlist?> GetWithProducts(int id);
    }
}
