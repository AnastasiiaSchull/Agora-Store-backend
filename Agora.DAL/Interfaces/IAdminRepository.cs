using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IAdminRepository
    {
        Task<IQueryable<Admin>> GetAll();
        Task<Admin> GetByUserId(int id);
        Task<Admin> Get(int id);
        Task Create(Admin user);
        void Update(Admin user);
        Task Delete(int id);

    }
}
