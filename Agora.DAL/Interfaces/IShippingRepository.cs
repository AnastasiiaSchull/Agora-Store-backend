using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IShippingRepository
    {
        Task<IQueryable<Shipping>> GetAll();
        Task<Shipping> Get(int id);
        Task<Shipping> GetByOrderItem(int id);
        Task Create(Shipping item);
        Task Update(Shipping item);
        Task Delete(int id);
    }
}
