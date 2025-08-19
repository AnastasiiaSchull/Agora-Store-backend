using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Agora.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Agora.DAL.Repository
{
    public class ProductRepository: IProductRepository
    {
        private AgoraContext db;
        public ProductRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Product>> GetAll()
        {
            return db.Products;
        }

        public async Task<Product> Get(int id)
        {
            return await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Product> GetByName(string name)
        {
            return await db.Products.Where(p => p.Name == name).FirstOrDefaultAsync();
        }
        public async Task<IQueryable<Product>> GetProductsBySeller(int sellerId)
        {
            return db.Products.Where(p=> p.Store.SellerId == sellerId);
        }

        public Task<IQueryable<Product>> GetSimilar(int categoryId, int subcategoryId, int excludeId)
        {
            var query = db.Products.Where(p =>
                p.Id != excludeId &&
                (p.CategoryId == categoryId || p.SubcategoryId == subcategoryId));

            return Task.FromResult(query);
        }
        public async Task<IEnumerable<Product>> GetFiltredProducts(int storeId, string field, string value)
        {

            if (field == "name")
            {
                return db.Products.Where(product => product.Name.Contains(value.ToLower()));
            }
            else if (field == "status")
            {

                return db.Products.Where(product => product.IsAvailable);
            }
            else if (field == "id")
            {
                if (int.TryParse(value, out int productId))
                {
                    return db.Products.Where(product => product.Id == productId);
                }
                return Enumerable.Empty<Product>();
            }


            return null;

        }

        public async Task Create(Product product)
        {
            await db.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            db.Entry(product).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Product? product = await db.Products.FindAsync(id);
            if (product != null)
                db.Products.Remove(product);
        }
        public async Task<IEnumerable<Product>> Find(Expression<Func<Product, bool>> predicate)
        {
            return await db.Products.Where(predicate).ToListAsync();
        }

        public Task GetSimilar(int? categoryId, int? subcategoryId, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
