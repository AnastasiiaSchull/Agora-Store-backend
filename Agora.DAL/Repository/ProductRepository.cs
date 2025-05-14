using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            return await db.Products.FindAsync(id);
        }
        public async Task<Product> GetByName(string name)
        {
            return await db.Products.Where(p => p.Name == name).FirstOrDefaultAsync();
        }
        public async Task<IQueryable<Product>> GetProductsBySeller(int sellerId)
        {
            return db.Products.Where(p=> p.Store.SellerId == sellerId);
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
    }
}
