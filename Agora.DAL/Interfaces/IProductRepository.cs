using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<IQueryable<Product>> GetAll();
        Task<Product> Get(int id);
        Task<IQueryable<Product>> GetProductsBySeller(int sellerId);
        Task Create(Product item);
        void Update(Product item);
        Task Delete(int id);

        //дефолтный метод
        Task<IEnumerable<Product>> Find(Expression<Func<Product, bool>> predicate) => Task.FromResult<IEnumerable<Product>>(null);

    }
}
