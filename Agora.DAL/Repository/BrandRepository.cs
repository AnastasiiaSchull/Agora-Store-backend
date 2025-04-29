using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Agora.DAL.Repository
{
    public class BrandRepository : IRepository<Brand>
    {
        private AgoraContext db;
        public BrandRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Brand>> GetAll()
        {
            return db.Brands;
        }

        public async Task<Brand> Get(int id)
        {
            return await db.Brands.FindAsync(id);
        }

        public async Task Create(Brand brand)
        {
            await db.Brands.AddAsync(brand);
        }

        public void Update(Brand brand)
        {
            db.Entry(brand).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Brand? brand = await db.Brands.FindAsync(id);
            if (brand != null)
                db.Brands.Remove(brand);
        }

        public async Task<IEnumerable<Brand>> Find(Expression<Func<Brand, bool>> predicate)
        {
            return await db.Brands.Where(predicate).ToListAsync();
        }
    }
}