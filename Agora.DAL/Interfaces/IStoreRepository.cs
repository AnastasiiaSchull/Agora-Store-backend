using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IStoreRepository
    {
        Task<IQueryable<Store>> GetAll();
        Task<Store> Get(int id);
        Task Create(Store item);
        void Update(Store item);
        Task Delete(int id);
    }
}
