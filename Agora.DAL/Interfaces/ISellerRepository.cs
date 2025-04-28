using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface ISellerRepository
    {
        Task<IQueryable<Seller>> GetAll();
        Task<Seller> GetByUserId(int id);
        Task<Seller> Get(int id);
        Task Create(Seller seller);
        void Update(Seller seller);
        Task Delete(int id);
    }
}
