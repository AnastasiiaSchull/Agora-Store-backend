using Agora.DAL.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        Task<Seller?> FindWithIncludes(
            Expression<Func<Seller, bool>> predicate,
            Func<IQueryable<Seller>, IIncludableQueryable<Seller, object>> include);
    }
}
