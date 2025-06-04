using System.Linq.Expressions;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<IQueryable<Product>> GetAll();
        Task<Product> Get(int id);
        Task<Product> GetByName(string name);
        Task<IQueryable<Product>> GetProductsBySeller(int sellerId);
        Task<IQueryable<Product>> GetSimilar(int categoryId, int subcategoryId, int excludeId);
        Task Create(Product item);
        void Update(Product item);
        Task Delete(int id);

        //дефолтный метод
        Task<IEnumerable<Product>> Find(Expression<Func<Product, bool>> predicate) => Task.FromResult<IEnumerable<Product>>(null);
        Task GetSimilar(int? categoryId, int? subcategoryId, int productId);
    }
}
